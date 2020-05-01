using System;

namespace SunCalcSharp
{
    public class SunTimes
    {
        /// <summary>
        /// sunrise (top edge of the sun appears on the horizon)
        /// </summary>
        public DateTime Sunrise;

        /// <summary>
        /// sunrise ends (bottom edge of the sun touches the horizon)
        /// </summary>
        public DateTime SunriseEnd;

        /// <summary>
        /// morning golden hour (soft light, best time for photography) ends
        /// </summary>
        public DateTime GoldenHourEnd;

        /// <summary>
        /// solar noon (sun is in the highest position)
        /// </summary>
        public DateTime SolarNoon;

        /// <summary>
        /// evening golden hour starts
        /// </summary>
        public DateTime GoldenHour;

        /// <summary>
        /// sunset starts (bottom edge of the sun touches the horizon)
        /// </summary>
        public DateTime SunsetStart;

        /// <summary>
        /// sunset (sun disappears below the horizon, evening civil twilight starts)
        /// </summary>
        public DateTime Sunset;

        /// <summary>
        /// dusk (evening nautical twilight starts)
        /// </summary>
        public DateTime Dusk;

        /// <summary>
        /// nautical dusk (evening astronomical twilight starts)
        /// </summary>
        public DateTime NauticalDusk;

        /// <summary>
        /// night starts (dark enough for astronomical observations)
        /// </summary>
        public DateTime Night;

        /// <summary>
        /// nadir (darkest moment of the night, sun is in the lowest position)
        /// </summary>
        public DateTime Nadir;

        /// <summary>
        /// night ends (morning astronomical twilight starts)
        /// </summary>
        public DateTime NightEnd;

        /// <summary>
        /// nautical dawn (morning nautical twilight starts)
        /// </summary>
        public DateTime NauticalDawn;

        /// <summary>
        /// dawn (morning nautical twilight ends, morning civil twilight starts)
        /// </summary>
        public DateTime Dawn;
    }
}