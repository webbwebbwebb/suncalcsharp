using System;
using System.Collections.Generic;
using SunCalcSharp.Dtos;
using SunCalcSharp.Formulas;

namespace SunCalcSharp
{
    public static class SunCalc
    {
        // sun times configuration (angle, morning name, evening name)
        private static readonly List<SunPositionTime> Times = new List<SunPositionTime>
        {
            new SunPositionTime(-0.833, SunPhaseNames.Sunrise, SunPhaseNames.Sunset),
            new SunPositionTime(-0.3, SunPhaseNames.SunriseEnd, SunPhaseNames.SunsetStart),
            new SunPositionTime(-6, SunPhaseNames.Dawn, SunPhaseNames.Dusk),
            new SunPositionTime(-12, SunPhaseNames.NauticalDawn, SunPhaseNames.NauticalDusk),
            new SunPositionTime(-18, SunPhaseNames.NightEnd, SunPhaseNames.Night),
            new SunPositionTime(6, SunPhaseNames.GoldenHourEnd, SunPhaseNames.GoldenHour)
        };

        // sun calculations are based on http://aa.quae.nl/en/reken/zonpositie.html formulas

        // calculates sun position for a given date and latitude/longitude
        public static SunPosition GetPosition(DateTime date, double lat, double lng)
        {
            double lw = Constants.Rad * -lng;
            double phi = Constants.Rad * lat;
            double d = Calendar.ToDays(date);

            SunCoordinates c = Sun.Coordinates(d);
            double H = Position.SiderealTime(d, lw) - c.RightAscension;

            return new SunPosition
            {
                Azimuth = Position.Azimuth(H, phi, c.Declination),
                Altitude = Position.Altitude(H, phi, c.Declination)
            };
        }

        // calculates sun times for a given date, latitude/longitude, and, optionally,
        // the observer height (in meters) relative to the horizon
        public static Dictionary<string, DateTime> GetTimes(DateTime date, double lat, double lng, double height = 0)
        {
            var lw = Constants.Rad * -lng;
            var phi = Constants.Rad * lat;

            var dh = Sun.ObserverAngle(height);

            var d = Calendar.ToDays(date);
            var n = Sun.JulianCycle(d, lw);
            var ds = Sun.ApproxTransit(0, lw, n);

            var M = Sun.SolarMeanAnomaly(ds);
            var L = Sun.EclipticLongitude(M);
            var dec = Position.Declination(L, 0);

            var Jnoon = Sun.SolarTransitJ(ds, M, L);

            var result = new Dictionary<string, DateTime>
            {
                {SunPhaseNames.SolarNoon, Calendar.FromJulian(Jnoon)},
                {SunPhaseNames.Nadir, Calendar.FromJulian(Jnoon - 0.5)},
            };

            for (int i = 0, len = Times.Count; i < len; i += 1)
            {
                var time = Times[i];
                var h0 = (time.Angle + dh) * Constants.Rad;
                var Jset = Sun.GetSetJ(h0, lw, phi, dec, n, M, L);
                var Jrise = Jnoon - (Jset - Jnoon);

                result.Add(time.RiseName, Calendar.FromJulian(Jrise));
                result.Add(time.SetName, Calendar.FromJulian(Jset));
            }

            return result;
        }
    }
}