using System;

namespace SunCalcSharp.Formulas
{
    internal static class Sun
    {
        // general sun calculations

        public static double SolarMeanAnomaly(double d)
        {
            return Constants.Rad * (357.5291 + 0.98560028 * d);
        }

        public static double EclipticLongitude(double M)
        {
            var C = Constants.Rad * (1.9148 * Math.Sin(M) + 0.02 * Math.Sin(2 * M) +
                                     0.0003 * Math.Sin(3 * M)); // equation of center
            var P = Constants.Rad * 102.9372; // perihelion of the Earth

            return M + C + P + Math.PI;
        }

        public static SunCoordinates Coordinates(double d)
        {
            var M = SolarMeanAnomaly(d);
            var L = EclipticLongitude(M);

            return new SunCoordinates()
            {
                Declination = Position.Declination(L, 0),
                RightAscension = Position.RightAscension(L, 0)
            };
        }

        // calculations for sun times

        private const double J0 = 0.0009;

        public static double JulianCycle(double d, double lw)
        {
            return Math.Round(d - J0 - lw / (2 * Math.PI));
        }

        public static double ApproxTransit(double Ht, double lw, double n)
        {
            return J0 + (Ht + lw) / (2 * Math.PI) + n;
        }

        public static double SolarTransitJ(double ds, double M, double L)
        {
            return Calendar.J2000 + ds + 0.0053 * Math.Sin(M) - 0.0069 * Math.Sin(2 * L);
        }

        public static double HourAngle(double h, double phi, double d)
        {
            return Math.Acos((Math.Sin(h) - Math.Sin(phi) * Math.Sin(d)) / (Math.Cos(phi) * Math.Cos(d)));
        }

        public static double ObserverAngle(double height)
        {
            return -2.076 * Math.Sqrt(height) / 60;
        }

        // returns set time for the given sun altitude
        public static double GetSetJ(double h, double lw, double phi, double dec, double n, double M, double L)
        {
            var w = HourAngle(h, phi, dec);
            var a = ApproxTransit(w, lw, n);

            return SolarTransitJ(a, M, L);
        }
    }
}