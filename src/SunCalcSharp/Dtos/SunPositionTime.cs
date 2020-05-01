using System;

namespace SunCalcSharp.Dtos
{
    internal class SunPositionTime
    {
        public SunPositionTime(double angle, Action<SunTimes, DateTime> setRiseProperty, Action<SunTimes, DateTime> setSetProperty)
        {
            Angle = angle;
            SetRiseProperty = setRiseProperty;
            SetSetProperty = setSetProperty;
        }

        public double Angle;
        public Action<SunTimes, DateTime> SetRiseProperty;
        public Action<SunTimes, DateTime> SetSetProperty;
    }
}