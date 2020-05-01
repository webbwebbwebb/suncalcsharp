using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SunCalcSharp.Tests
{
    [TestClass]
    public class SunCalcTests
    {
        private readonly DateTime _date = DateTime.SpecifyKind(DateTime.Parse("2013-03-05"), DateTimeKind.Utc);
        private readonly double _lat = 50.5;
        private readonly double _lng = 30.5;
        private readonly double _height = 2000;

        [TestMethod]
        public void GetPosition_returns_azimuth_for_the_given_time_and_location()
        {
            var sunPosition = SunCalc.GetPosition(_date, _lat, _lng);
            sunPosition.Azimuth.Should().BeApproximately(-2.5003175907168385, 1e-15);
        }

        [TestMethod]
        public void GetPosition_returns_altitude_for_the_given_time_and_location()
        {
            var sunPosition = SunCalc.GetPosition(_date, _lat, _lng);
            sunPosition.Altitude.Should().BeApproximately(-0.7000406838781611, 1e-15);
        }

        [TestMethod]
        public void GetTimes_returns_SolarNoon_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.SolarNoon.ToDateString().Should().Be("2013-03-05T10:10:57Z");
        }

        [TestMethod]
        public void GetTimes_returns_Nadir_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Nadir.ToDateString().Should().Be("2013-03-04T22:10:57Z");
        }

        
        [TestMethod]
        public void GetTimes_returns_Sunrise_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Sunrise.ToDateString().Should().Be("2013-03-05T04:34:56Z");
        }

        [TestMethod]
        public void GetTimes_returns_Sunset_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Sunset.ToDateString().Should().Be("2013-03-05T15:46:57Z");
        }

        [TestMethod]
        public void GetTimes_returns_SunriseEnd_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.SunriseEnd.ToDateString().Should().Be("2013-03-05T04:38:19Z");
        }

        [TestMethod]
        public void GetTimes_returns_SunsetStart_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.SunsetStart.ToDateString().Should().Be("2013-03-05T15:43:34Z");
        }

        [TestMethod]
        public void GetTimes_returns_Dawn_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Dawn.ToDateString().Should().Be("2013-03-05T04:02:17Z");
        }

        [TestMethod]
        public void GetTimes_returns_Dusk_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Dusk.ToDateString().Should().Be("2013-03-05T16:19:36Z");
        }

        [TestMethod]
        public void GetTimes_returns_NauticalDawn_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.NauticalDawn.ToDateString().Should().Be("2013-03-05T03:24:31Z");
        }

        [TestMethod]
        public void GetTimes_returns_NauticalDusk_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.NauticalDusk.ToDateString().Should().Be("2013-03-05T16:57:22Z");
        }

        [TestMethod]
        public void GetTimes_returns_NightEnd_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.NightEnd.ToDateString().Should().Be("2013-03-05T02:46:17Z");
        }

        [TestMethod]
        public void GetTimes_returns_Night_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.Night.ToDateString().Should().Be("2013-03-05T17:35:36Z");
        }

        [TestMethod]
        public void GetTimes_returns_GoldenHourEnd_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.GoldenHourEnd.ToDateString().Should().Be("2013-03-05T05:19:01Z");
        }

        [TestMethod]
        public void GetTimes_returns_GoldenHour_for_the_given_date_and_location()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng);

            times.GoldenHour.ToDateString().Should().Be("2013-03-05T15:02:52Z");
        }

        [TestMethod]
        public void GetTimes_adjusts_sun_phases_when_additionally_given_the_observer_height()
        {
            var times = SunCalc.GetTimes(_date, _lat, _lng, _height);

            times.SolarNoon.ToDateString().Should().Be("2013-03-05T10:10:57Z");
            times.Nadir.ToDateString().Should().Be("2013-03-04T22:10:57Z");
            times.Sunrise.ToDateString().Should().Be("2013-03-05T04:25:07Z");
            times.Sunset.ToDateString().Should().Be("2013-03-05T15:56:46Z");
        }
    }
}
