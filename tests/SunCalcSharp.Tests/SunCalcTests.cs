using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SunCalcSharp.Tests
{
    [TestClass]
    public class SunCalcTests
    {
        private DateTime date = DateTime.SpecifyKind(DateTime.Parse("2013-03-05"), DateTimeKind.Utc);
        private double lat = 50.5;
        private double lng = 30.5;
        private double height = 2000;

        [TestMethod]
        public void GetPosition_returns_azimuth_for_the_given_time_and_location()
        {
            var sunPosition = SunCalc.GetPosition(date, lat, lng);
            sunPosition.azimuth.Should().BeApproximately(-2.5003175907168385, 1e-15);
        }

        [TestMethod]
        public void GetPosition_returns_altitude_for_the_given_time_and_location()
        {
            var sunPosition = SunCalc.GetPosition(date, lat, lng);
            sunPosition.altitude.Should().BeApproximately(-0.7000406838781611, 1e-15);
        }

        [DataTestMethod]
        [DataRow(SunPhaseNames.SolarNoon, "2013-03-05T10:10:57Z")]
        [DataRow(SunPhaseNames.Nadir, "2013-03-04T22:10:57Z")]
        [DataRow(SunPhaseNames.Sunrise, "2013-03-05T04:34:56Z")]
        [DataRow(SunPhaseNames.Sunset, "2013-03-05T15:46:57Z")]
        [DataRow(SunPhaseNames.SunriseEnd, "2013-03-05T04:38:19Z")]
        [DataRow(SunPhaseNames.SunsetStart, "2013-03-05T15:43:34Z")]
        [DataRow(SunPhaseNames.Dawn, "2013-03-05T04:02:17Z")]
        [DataRow(SunPhaseNames.Dusk, "2013-03-05T16:19:36Z")]
        [DataRow(SunPhaseNames.NauticalDawn, "2013-03-05T03:24:31Z")]
        [DataRow(SunPhaseNames.NauticalDusk, "2013-03-05T16:57:22Z")]
        [DataRow(SunPhaseNames.NightEnd, "2013-03-05T02:46:17Z")]
        [DataRow(SunPhaseNames.Night, "2013-03-05T17:35:36Z")]
        [DataRow(SunPhaseNames.GoldenHourEnd, "2013-03-05T05:19:01Z")]
        [DataRow(SunPhaseNames.GoldenHour, "2013-03-05T15:02:52Z")]
        public void GetTimes_returns_sun_phases_for_the_given_date_and_location(string sunPhaseName, string expectedTime)
        {
            var times = SunCalc.GetTimes(date, lat, lng);

            times[sunPhaseName].ToString("yyyy-MM-ddTHH:mm:ssK").Should().Be(expectedTime);
        }

        [DataTestMethod]
        [DataRow(SunPhaseNames.SolarNoon, "2013-03-05T10:10:57Z")]
        [DataRow(SunPhaseNames.Nadir, "2013-03-04T22:10:57Z")]
        [DataRow(SunPhaseNames.Sunrise, "2013-03-05T04:25:07Z")]
        [DataRow(SunPhaseNames.Sunset, "2013-03-05T15:56:46Z")]
        public void GetTimes_adjusts_sun_phases_when_additionally_given_the_observer_height(string sunPhaseName, string expectedTime)
        {
            var times = SunCalc.GetTimes(date, lat, lng, height);

            times[sunPhaseName].ToString("yyyy-MM-ddTHH:mm:ssK").Should().Be(expectedTime);
        }

        [TestMethod]
        public void GetMoonPosition_returns_azimuth_given_time_and_location()
        {
            var moonPosition = SunCalc.GetMoonPosition(date, lat, lng);

            moonPosition.azimuth.Should().BeApproximately(-0.9783999522438226, 1e-15);
        }

        [TestMethod]
        public void GetMoonPosition_returns_altitude_given_time_and_location()
        {
            var moonPosition = SunCalc.GetMoonPosition(date, lat, lng);

            moonPosition.altitude.Should().BeApproximately(0.014551482243892251, 1e-15);
        }

        [TestMethod]
        public void GetMoonPosition_returns_distance_given_time_and_location()
        {
            var moonPosition = SunCalc.GetMoonPosition(date, lat, lng);

            moonPosition.distance.Should().BeApproximately(364121.37256256194, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_fraction_of_moons_illuminated_limb()
        {
            var moonIllumination = SunCalc.GetMoonIllumination(date);

            moonIllumination.fraction.Should().BeApproximately(0.4848068202456373, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_angle_of_moons_illuminated_limb()
        {
            var moonIllumination = SunCalc.GetMoonIllumination(date);

            moonIllumination.angle.Should().BeApproximately(1.6732942678578346, 1e-15);
        }

        [TestMethod]
        public void GetMoonIllumination_returns_phase_of_moon()
        {
            var moonIllumination = SunCalc.GetMoonIllumination(date);

            moonIllumination.phase.Should().BeApproximately(0.7548368838538762, 1e-15);
        }

        [TestMethod]
        public void GetMoonTimes_returns_moon_rise_time_given_date_and_location()
        {
            var moonTimes = SunCalc.GetMoonTimes(date.AddDays(-1), lat, lng);

            moonTimes.rise.ToString("yyyy-MM-ddTHH:mm:ssK").Should().Be("2013-03-04T23:54:29Z");
        }

        [TestMethod]
        public void GetMoonTimes_returns_moon_set_time_given_date_and_location()
        {
            var moonTimes = SunCalc.GetMoonTimes(date.AddDays(-1), lat, lng);

            moonTimes.set.ToString("yyyy-MM-ddTHH:mm:ssK").Should().Be("2013-03-04T07:47:58Z");
        }
    }
}
