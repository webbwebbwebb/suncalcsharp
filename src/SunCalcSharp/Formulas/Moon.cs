using System;

namespace SunCalcSharp.Formulas
{
    internal static class Moon
    {
        public static MoonCoordinates Coordinates(double d)
        {
            var L = Constants.Rad * (218.316 + 13.176396 * d); // ecliptic longitude
            var M = Constants.Rad * (134.963 + 13.064993 * d); // mean anomaly
            var F = Constants.Rad * (93.272 + 13.229350 * d); // mean distance

            var l = L + Constants.Rad * 6.289 * Math.Sin(M); // longitude
            var b = Constants.Rad * 5.128 * Math.Sin(F);     // latitude
            var dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km

            return new MoonCoordinates
            {
                RightAscension = Position.RightAscension(l, b),
                Declination = Position.Declination(l, b),
                Distance = dt
            };
        }

        public static double AstroRefraction(double h)
        {
            if (h < 0) // the following formula works for positive altitudes only.
                h = 0; // if h = -0.08901179 a div/0 would occur.

            // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
            return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
        }
    }
}