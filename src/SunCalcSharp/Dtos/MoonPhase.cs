namespace SunCalcSharp
{
    /// <summary>
    /// The eight principal and intermediate phases of the Moon
    /// </summary>
    public enum MoonPhase
    {
        /// <summary>
        /// Completely in Sun's shadow
        /// </summary>
        NewMoon,

        /// <summary>
        /// Disc between 0.1% and 49.9% lit, and increasing
        /// </summary>
        WaxingCrescent,

        /// <summary>
        /// Disc 50% lit
        /// </summary>
        FirstQuarter,

        /// <summary>
        /// Disc between 50.1% and 99.9% lit, and increasing
        /// </summary>
        WaxingGibbous,
        
        /// <summary>
        /// 100% illuminated
        /// </summary>
        FullMoon,

        /// <summary>
        /// Disc between 99.9% and 50.1% lit, and decreasing
        /// </summary>
        WaningGibbous,

        /// <summary>
        /// 50% lit
        /// </summary>
        LastQuarter,

        /// <summary>
        /// Disc between 49.9% and 0.1% lit, and decreasing
        /// </summary>
        WaningCrescent
    }

    /// <summary>
    /// Maps moon phases
    /// </summary>
    public static class MoonPhaseMapper
    {
        /// <summary>
        /// Map moon phase based on how far the moon is through the lunar cycle
        /// </summary>
        /// <param name="progressThroughCycle">percentage value in the range 0 to 1, defining how far the moon is through the lunar cycle</param>
        /// <returns><see cref="MoonPhase"/></returns>
        public static MoonPhase FromProgressThroughCycle(double progressThroughCycle)
        {
            if (progressThroughCycle == 0)
            {
                return MoonPhase.NewMoon;
            }

            if (progressThroughCycle > 0 && progressThroughCycle < 0.25)
            {
                return MoonPhase.WaxingCrescent;
            }

            if (progressThroughCycle == 0.5)
            {
                return MoonPhase.FullMoon;
            }

            if (progressThroughCycle > 0.5 && progressThroughCycle < 0.75)
            {
                return MoonPhase.WaningGibbous;
            }

            if (progressThroughCycle == 0.75)
            {
                return MoonPhase.LastQuarter;
            }

            return MoonPhase.WaningCrescent;
        }
    }
}