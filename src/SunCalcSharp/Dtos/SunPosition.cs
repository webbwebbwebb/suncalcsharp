namespace SunCalcSharp
{
    /// <summary>
    /// Contains properties relating to the position of the sun
    /// </summary>
    public class SunPosition
    {
        /// <summary>
        /// Solar azimuth in radians (direction along the horizon, measured from south to west), e.g. 0 is south and Math.PI * 3/4 is northwest
        /// </summary>
        public double Azimuth;

        /// <summary>
        /// Solar altitude above the horizon in radians, e.g. 0 at the horizon and PI/2 at the zenith (straight over your head)
        /// </summary>
        public double Altitude;
    }
}