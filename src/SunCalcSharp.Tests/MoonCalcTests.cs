using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SunCalcSharp.Tests
{
    [TestClass]
    public class MoonCalcTests
    {
        private readonly  DateTime _date = DateTime.SpecifyKind(DateTime.Parse("2013-03-05"), DateTimeKind.Utc);
        private readonly  double _lat = 50.5;
        private readonly  double _lng = 30.5;

        [TestMethod]
        public void GetMoonPosition_returns_azimuth_given_time_and_location()
        {
            var moonPosition = MoonCalc.GetMoonPosition(_date, _lat, _lng);

            moonPosition.Azimuth.Should().BeApproximately(-0.9783999522438226, 1e-15);
        }

        [TestMethod]
        public void GetMoonPosition_returns_altitude_given_time_and_location()
        {
            var moonPosition = MoonCalc.GetMoonPosition(_date, _lat, _lng);

            moonPosition.Altitude.Should().BeApproximately(0.014551482243892251, 1e-15);
        }

        [TestMethod]
        public void GetMoonPosition_returns_distance_given_time_and_location()
        {
            var moonPosition = MoonCalc.GetMoonPosition(_date, _lat, _lng);

            moonPosition.Distance.Should().BeApproximately(364121.37256256194, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_fraction_of_moons_illuminated_limb()
        {
            var moonIllumination = MoonCalc.GetMoonIllumination(_date);

            moonIllumination.Fraction.Should().BeApproximately(0.4848068202456373, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_angle_of_moons_illuminated_limb()
        {
            var moonIllumination = MoonCalc.GetMoonIllumination(_date);

            moonIllumination.Angle.Should().BeApproximately(1.6732942678578346, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_phase_of_moon()
        {
            var moonIllumination = MoonCalc.GetMoonIllumination(_date);

            moonIllumination.Phase.Should().BeApproximately(0.7548368838538762, 1e-15);
        }

        [TestMethod]
        public void GetMoonTimes_returns_moon_rise_time_given_date_and_location()
        {
            var moonTimes = MoonCalc.GetMoonTimes(_date.AddDays(-1), _lat, _lng);

            moonTimes.Rise.ToDateString().Should().Be("2013-03-04T23:54:29Z");
        }

        [TestMethod]
        public void GetMoonTimes_returns_moon_set_time_given_date_and_location()
        {
            var moonTimes = MoonCalc.GetMoonTimes(_date.AddDays(-1), _lat, _lng);

            moonTimes.Set.ToDateString().Should().Be("2013-03-04T07:47:58Z");
        }
    }
}
