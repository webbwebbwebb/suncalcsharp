using System;

namespace SunCalcSharp.Tests
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime? dateTime)
        {
            return dateTime?.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}
