using System;
using System.Collections.Generic;
using System.Text;

namespace SunCalcSharp.Tests
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
        }
    }
}
