using System;

namespace SunCalcSharp
{
    /// <summary>
    /// Contains times for when the moon rises and sets each day
    /// </summary>
    public class MoonTimes
    {
        /// <summary>
        /// Time of moon rise
        /// </summary>
        public DateTime Rise;

        /// <summary>
        /// Time of moon set
        /// </summary>
        public DateTime Set;

        /// <summary>
        /// True if the moon never rises/sets and is always above the horizon during the day
        /// </summary>
        public bool AlwaysUp;

        /// <summary>
        /// True if the moon is always below the horizon
        /// </summary>
        public bool AlwaysDown;
    }
}