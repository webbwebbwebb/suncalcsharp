using System;

namespace SunCalcSharp
{
    internal static class SunCalculations
    {
        // general sun calculations

        public static double solarMeanAnomaly(double d)
        {
            return Constants.rad * (357.5291 + 0.98560028 * d);
        }

        public static double eclipticLongitude(double M)
        {
            var C = Constants.rad * (1.9148 * Math.Sin(M) + 0.02 * Math.Sin(2 * M) +
                                     0.0003 * Math.Sin(3 * M)); // equation of center
            var P = Constants.rad * 102.9372; // perihelion of the Earth

            return M + C + P + Math.PI;
        }

        public static SunCoordinates sunCoords(double d)
        {
            var M = solarMeanAnomaly(d);
            var L = eclipticLongitude(M);

            return new SunCoordinates()
            {
                dec = PositionCalculations.declination(L, 0),
                ra = PositionCalculations.rightAscension(L, 0)
            };
        }

        // calculations for sun times

        private const double J0 = 0.0009;

        public static double julianCycle(double d, double lw)
        {
            return Math.Round(d - J0 - lw / (2 * Math.PI));
        }

        public static double approxTransit(double Ht, double lw, double n)
        {
            return J0 + (Ht + lw) / (2 * Math.PI) + n;
        }

        public static double solarTransitJ(double ds, double M, double L)
        {
            return Calendar.J2000 + ds + 0.0053 * Math.Sin(M) - 0.0069 * Math.Sin(2 * L);
        }

        public static double hourAngle(double h, double phi, double d)
        {
            return Math.Acos((Math.Sin(h) - Math.Sin(phi) * Math.Sin(d)) / (Math.Cos(phi) * Math.Cos(d)));
        }

        public static double observerAngle(double height)
        {
            return -2.076 * Math.Sqrt(height) / 60;
        }

        // returns set time for the given sun altitude
        public static double getSetJ(double h, double lw, double phi, double dec, double n, double M, double L)
        {
            var w = hourAngle(h, phi, dec);
            var a = approxTransit(w, lw, n);

            return solarTransitJ(a, M, L);
        }
    }
}