using System;
using System.Collections.Generic;

namespace SunCalcSharp
{
    public static class SunCalc
    {
        // sun times configuration (angle, morning name, evening name)
        private static List<SunPositionTime> times = new List<SunPositionTime>
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
            double lw = Constants.rad * -lng;
            double phi = Constants.rad * lat;
            double d = Calendar.toDays(date);

            SunCoordinates c = SunCalculations.sunCoords(d);
            double H = PositionCalculations.siderealTime(d, lw) - c.ra;

            return new SunPosition
            {
                azimuth = PositionCalculations.azimuth(H, phi, c.dec),
                altitude = PositionCalculations.altitude(H, phi, c.dec)
            };
        }

        // calculates sun times for a given date, latitude/longitude, and, optionally,
        // the observer height (in meters) relative to the horizon
        public static Dictionary<string, DateTime> GetTimes(DateTime date, double lat, double lng, double height = 0)
        {
            var lw = Constants.rad * -lng;
            var phi = Constants.rad * lat;

            var dh = SunCalculations.observerAngle(height);

            var d = Calendar.toDays(date);
            var n = SunCalculations.julianCycle(d, lw);
            var ds = SunCalculations.approxTransit(0, lw, n);

            var M = SunCalculations.solarMeanAnomaly(ds);
            var L = SunCalculations.eclipticLongitude(M);
            var dec = PositionCalculations.declination(L, 0);

            var Jnoon = SunCalculations.solarTransitJ(ds, M, L);

            var result = new Dictionary<string, DateTime>
            {
                {SunPhaseNames.SolarNoon, Calendar.fromJulian(Jnoon)},
                {SunPhaseNames.Nadir, Calendar.fromJulian(Jnoon - 0.5)},
            };

            for (int i = 0, len = times.Count; i < len; i += 1)
            {
                var time = times[i];
                var h0 = (time.angle + dh) * Constants.rad;
                var Jset = SunCalculations.getSetJ(h0, lw, phi, dec, n, M, L);
                var Jrise = Jnoon - (Jset - Jnoon);

                result.Add(time.riseName, Calendar.fromJulian(Jrise));
                result.Add(time.setName, Calendar.fromJulian(Jset));
            }

            return result;
        }
    }
}