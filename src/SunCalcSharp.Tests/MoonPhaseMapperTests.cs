using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SunCalcSharp.Tests
{
    [TestClass]
    public class MoonPhaseMapperTests
    {
        [TestMethod]
        public void FromProgressThroughCycle_when_progress_is_0_returns_NewMoon()
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(0);

            moonPhase.Should().Be(MoonPhase.NewMoon);
        }

        [DataTestMethod]
        [DataRow(0.01)]
        [DataRow(0.24)]
        public void FromProgressThroughCycle_when_progress_is_greater_than_0_and_less_than_0_25_returns_WaxingCrescent(double progress)
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(progress);

            moonPhase.Should().Be(MoonPhase.WaxingCrescent);
        }

        [TestMethod]
        public void FromProgressThroughCycle_when_progress_is_0_25_returns_FirstQuarter()
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(0.25);

            moonPhase.Should().Be(MoonPhase.FirstQuarter);
        }

        [DataTestMethod]
        [DataRow(0.26)]
        [DataRow(0.49)]
        public void FromProgressThroughCycle_when_progress_is_greater_than_0_25_and_less_than_0_5_returns_WaxingGibbous(double progress)
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(progress);

            moonPhase.Should().Be(MoonPhase.WaxingGibbous);
        }

        [TestMethod]
        public void FromProgressThroughCycle_when_progress_is_0_5_returns_FullMoon()
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(0.5);

            moonPhase.Should().Be(MoonPhase.FullMoon);
        }

        [DataTestMethod]
        [DataRow(0.51)]
        [DataRow(0.74)]
        public void FromProgressThroughCycle_when_progress_is_greater_than_0_5_and_less_than_0_75_returns_WaningGibbous(double progress)
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(progress);

            moonPhase.Should().Be(MoonPhase.WaningGibbous);
        }

        [TestMethod]
        public void FromProgressThroughCycle_when_progress_is_0_75_returns_LastQuarter()
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(0.75);

            moonPhase.Should().Be(MoonPhase.LastQuarter);
        }

        [DataTestMethod]
        [DataRow(0.76)]
        [DataRow(1)]
        public void FromProgressThroughCycle_when_progress_is_greater_than_0_75_returns_WaningCrescent(double progress)
        {
            var moonPhase = MoonPhaseMapper.FromProgressThroughCycle(progress);

            moonPhase.Should().Be(MoonPhase.WaningCrescent);
        }
    }
}