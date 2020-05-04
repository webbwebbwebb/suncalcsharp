namespace SunCalcSharp
{
    /// <summary>
    /// Contains properties relating to the illumination of the moon
    /// </summary>
    public class MoonIllumination
    {
        /// <summary>
        /// Illuminated fraction of the moon; varies from 0.0 (new moon) to 1.0 (full moon)
        /// </summary>
        public double Fraction;

        /// <summary>
        /// Which of the eight principal and intermediate phases the moon is currently in
        /// </summary>
        public MoonPhase Phase;

        /// <summary>
        /// Midpoint angle in radians of the illuminated limb of the moon reckoned eastward from the north point of the disk;
        /// the moon is waxing if the angle is negative, and waning if positive
        /// </summary>
        public double Angle;
    }
}