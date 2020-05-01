using System;

namespace SunCalcSharp.Formulas
{
    internal static class Calendar
    {
        // date/time constants and conversions
        public const double dayMs = 86400000;
        public const double J1970 = 2440588;
        public const double J2000 = 2451545;

        public static double ToJulian(DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeMilliseconds() / dayMs - 0.5 + J1970;
        }

        public static DateTime FromJulian(double j)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds((long)((j + 0.5 - J1970) * dayMs)).UtcDateTime;
        }

        public static double ToDays(DateTime date)
        {
            return ToJulian(date) - J2000;
        }
    }
}