namespace SunCalcSharp
{
    public class MoonIllumination
    {
        /// <summary>
        /// Illuminated fraction of the moon; varies from 0.0 (new moon) to 1.0 (full moon)
        /// </summary>
        public double Fraction;

        /// <summary>
        /// Moon phase; varies from 0.0 to 1.0
        /// 0 - new moon
        /// >0 and <0.25   - waxing crescent
        /// 0.25           - first quarter
        /// >0.25 and <0.5 - waxing gibbous
        /// 0.5            - full moon
        /// >0.5 and <0.75 - waning gibbous
        /// 0.75           - last quarter
        /// >0.75          - waning crescent
        /// </summary>
        public double Phase;

        /// <summary>
        /// Midpoint angle in radians of the illuminated limb of the moon reckoned eastward from the north point of the disk;
        /// the moon is waxing if the angle is negative, and waning if positive
        /// </summary>
        public double Angle;
    }
}