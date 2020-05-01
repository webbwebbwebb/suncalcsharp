using System;
using System.Collections.Generic;
using SunCalcSharp.Formulas;

namespace SunCalcSharp
{
    public static class SunCalc
    {
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
        public static SunTimes GetTimes(DateTime date, double lat, double lng, double height = 0)
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

            var result = new SunTimes
            {
                SolarNoon = Calendar.FromJulian(Jnoon),
                Nadir = Calendar.FromJulian(Jnoon - 0.5)
            };

            for (int i = 0, len = Times.Count; i < len; i += 1)
            {
                var time = Times[i];
                var h0 = (time.Angle + dh) * Constants.Rad;
                var Jset = Sun.GetSetJ(h0, lw, phi, dec, n, M, L);
                var Jrise = Jnoon - (Jset - Jnoon);

                time.SetRiseProperty(result, Calendar.FromJulian(Jrise));
                time.SetSetProperty(result, Calendar.FromJulian(Jset));
            }

            return result;
        }

        // sun times configuration (angle, morning name, evening name)
        private static readonly List<SunAngleTime> Times = new List<SunAngleTime>
        {
            new SunAngleTime(-0.833, (t, v) => t.Sunrise = v, (t, v) => t.Sunset = v),
            new SunAngleTime(-0.3, (t, v) => t.SunriseEnd = v, (t, v) => t.SunsetStart = v),
            new SunAngleTime(-6, (t, v) => t.Dawn = v, (t, v) => t.Dusk= v),
            new SunAngleTime(-12, (t, v) => t.NauticalDawn= v, (t, v) => t.NauticalDusk= v), 
            new SunAngleTime(-18, (t, v) => t.NightEnd= v, (t, v) => t.Night= v),
            new SunAngleTime(6, (t, v) => t.GoldenHourEnd= v, (t, v) => t.GoldenHour= v)
        };
    }
}