using System;

namespace SunCalcSharp
{
    internal static class PositionCalculations
    {
        // general calculations for position
        private const double e = (Math.PI / 180) * 23.4397; // obliquity of the Earth

        public static double rightAscension(double l, double b)
        {
            return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l));
        }

        public static double declination(double l, double b)
        {
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l));
        }

        public static double azimuth(double H, double phi, double dec)
        {
            return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi));
        }

        public static double altitude(double H, double phi, double dec)
        {
            return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H));
        }

        public static double siderealTime(double d, double lw)
        {
            return Constants.rad * (280.16 + 360.9856235 * d) - lw;
        }
    }
}