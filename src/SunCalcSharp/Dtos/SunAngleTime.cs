using System;

namespace SunCalcSharp
{
    internal class SunAngleTime
    {
        public SunAngleTime(double angle, Action<SunTimes, DateTime> setRiseProperty, Action<SunTimes, DateTime> setSetProperty)
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