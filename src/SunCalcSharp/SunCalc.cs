using System;
using System.Collections.Generic;

namespace SunCalcSharp
{
    public static class SunCalc
    {
        private static double PI = Math.PI;
        private static double rad = Math.PI / 180;

        // sun calculations are based on http://aa.quae.nl/en/reken/zonpositie.html formulas

        // date/time constants and conversions
        private const double dayMs = 86400000;
        private const double J1970 = 2440588;
        private const double J2000 = 2451545;

        private static double toJulian(DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeMilliseconds() / dayMs - 0.5 + J1970;
        }

        private static DateTime fromJulian(double j)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds((long)((j + 0.5 - J1970) * dayMs)).UtcDateTime;
        }

        private static double toDays(DateTime date)
        {
            return toJulian(date) - J2000;
        }

        // general calculations for position
        private const double e = (Math.PI / 180) * 23.4397; // obliquity of the Earth

        private static double rightAscension(double l, double b)
        {
            return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l));
        }

        private static double declination(double l, double b)
        {
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l));
        }

        private static double azimuth(double H, double phi, double dec)
        {
            return Math.Atan2(Math.Sin(H), Math.Cos(H) * Math.Sin(phi) - Math.Tan(dec) * Math.Cos(phi));
        }

        private static double altitude(double H, double phi, double dec)
        {
            return Math.Asin(Math.Sin(phi) * Math.Sin(dec) + Math.Cos(phi) * Math.Cos(dec) * Math.Cos(H));
        }

        private static double siderealTime(double d, double lw)
        {
            return rad * (280.16 + 360.9856235 * d) - lw;
        }

        private static double astroRefraction(double h)
        {
            if (h < 0) // the following formula works for positive altitudes only.
                h = 0; // if h = -0.08901179 a div/0 would occur.

            // formula 16.4 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            // 1.02 / tan(h + 10.26 / (h + 5.10)) h in degrees, result in arc minutes -> converted to rad:
            return 0.0002967 / Math.Tan(h + 0.00312536 / (h + 0.08901179));
        }

        // general sun calculations

        private static double solarMeanAnomaly(double d)
        {
            return rad * (357.5291 + 0.98560028 * d);
        }

        private static double eclipticLongitude(double M)
        {
            var C = rad * (1.9148 * Math.Sin(M) + 0.02 * Math.Sin(2 * M) +
                           0.0003 * Math.Sin(3 * M)); // equation of center
            var P = rad * 102.9372; // perihelion of the Earth

            return M + C + P + PI;
        }

        // calculates sun position for a given date and latitude/longitude

        public static SunPosition GetPosition(DateTime date, double lat, double lng)
        {
            double lw = rad * -lng;
            double phi = rad * lat;
            double d = toDays(date);

            SunCoordinates c = sunCoords(d);
            double H = siderealTime(d, lw) - c.ra;

            return new SunPosition
            {
                azimuth = azimuth(H, phi, c.dec),
                altitude = altitude(H, phi, c.dec)
            };
        }


        // sun times configuration (angle, morning name, evening name)

        private static List<SunPositionTime> times = new List<SunPositionTime>
        {
            new SunPositionTime(-0.833, SunPhaseNames.Sunrise, SunPhaseNames.Sunset),
            new SunPositionTime(-0.3, SunPhaseNames.SunriseEnd, SunPhaseNames.SunsetStart),
            new SunPositionTime(-6, SunPhaseNames.Dawn, SunPhaseNames.Dusk),
            new SunPositionTime(-12, SunPhaseNames.NauticalDawn, SunPhaseNames.NauticalDusk),
            new SunPositionTime(-18, SunPhaseNames.NightEnd, SunPhaseNames.Night),
            new SunPositionTime(6, SunPhaseNames.GoldenHourEnd, SunPhaseNames.GoldenHour)
        };

        private static SunCoordinates sunCoords(double d)
        {
            var M = solarMeanAnomaly(d);
            var L = eclipticLongitude(M);

            return new SunCoordinates()
            {
                dec = declination(L, 0),
                ra = rightAscension(L, 0)
            };
        }

        // calculations for sun times

        private const double J0 = 0.0009;

        private static double julianCycle(double d, double lw)
        {
            return Math.Round(d - J0 - lw / (2 * PI));
        }

        private static double approxTransit(double Ht, double lw, double n)
        {
            return J0 + (Ht + lw) / (2 * PI) + n;
        }

        private static double solarTransitJ(double ds, double M, double L)
        {
            return J2000 + ds + 0.0053 * Math.Sin(M) - 0.0069 * Math.Sin(2 * L);
        }

        private static double hourAngle(double h, double phi, double d)
        {
            return Math.Acos((Math.Sin(h) - Math.Sin(phi) * Math.Sin(d)) / (Math.Cos(phi) * Math.Cos(d)));
        }

        private static double observerAngle(double height)
        {
            return -2.076 * Math.Sqrt(height) / 60;
        }

        // returns set time for the given sun altitude
        private static double getSetJ(double h, double lw, double phi, double dec, double n, double M, double L)
        {
            var w = hourAngle(h, phi, dec);
            var a = approxTransit(w, lw, n);

            return solarTransitJ(a, M, L);
        }

        // calculates sun times for a given date, latitude/longitude, and, optionally,
        // the observer height (in meters) relative to the horizon

        public static Dictionary<string, DateTime> GetTimes(DateTime date, double lat, double lng, double height = 0)
        {
            var lw = rad * -lng;
            var phi = rad * lat;

            var dh = observerAngle(height);

            var d = toDays(date);
            var n = julianCycle(d, lw);
            var ds = approxTransit(0, lw, n);

            var M = solarMeanAnomaly(ds);
            var L = eclipticLongitude(M);
            var dec = declination(L, 0);

            var Jnoon = solarTransitJ(ds, M, L);

            var result = new Dictionary<string, DateTime>
            {
                {SunPhaseNames.SolarNoon, fromJulian(Jnoon)},
                {SunPhaseNames.Nadir, fromJulian(Jnoon - 0.5)},
            };

            for (int i = 0, len = times.Count; i < len; i += 1)
            {
                var time = times[i];
                var h0 = (time.angle + dh) * rad;
                var Jset = getSetJ(h0, lw, phi, dec, n, M, L);
                var Jrise = Jnoon - (Jset - Jnoon);

                result.Add(time.riseName, fromJulian(Jrise));
                result.Add(time.setName, fromJulian(Jset));
            }

            return result;
        }

        
        // moon calculations, based on http://aa.quae.nl/en/reken/hemelpositie.html formulas

        private static MoonCoordinates moonCoords(double d)
        {
            var L = rad * (218.316 + 13.176396 * d); // ecliptic longitude
            var M = rad * (134.963 + 13.064993 * d); // mean anomaly
            var F = rad * (93.272 + 13.229350 * d); // mean distance

            var l = L + rad * 6.289 * Math.Sin(M); // longitude
            var b = rad * 5.128 * Math.Sin(F);     // latitude
            var dt = 385001 - 20905 * Math.Cos(M);  // distance to the moon in km

            return new MoonCoordinates
            {
                ra = rightAscension(l, b),
                dec = declination(l, b),
                dist = dt
            };
        }

        public static MoonPosition GetMoonPosition(DateTime date, double lat, double lng)
        {
            var lw = rad * -lng;
            var phi = rad * lat;
            var d = toDays(date);

            var c = moonCoords(d);
            var H = siderealTime(d, lw) - c.ra;
            var h = altitude(H, phi, c.dec);
            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            var pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.dec) - Math.Sin(c.dec) * Math.Cos(H));

            h = h + astroRefraction(h); // altitude correction for refraction

            return new MoonPosition
            {
                azimuth = azimuth(H, phi, c.dec),
                altitude = h,
                distance = c.dist,
                parallacticAngle = pa
            };
        }

        // calculations for illumination parameters of the moon,
        // based on http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro formulas and
        // Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.

        public static MoonIllumination GetMoonIllumination(DateTime date)
        {
            var d = toDays(date);
            var s = sunCoords(d);
            var m = moonCoords(d);

            const int sdist = 149598000; // distance from Earth to Sun in km
            var phi = Math.Acos(Math.Sin(s.dec) * Math.Sin(m.dec) + Math.Cos(s.dec) * Math.Cos(m.dec) * Math.Cos(s.ra - m.ra));
            var inc = Math.Atan2(sdist * Math.Sin(phi), m.dist - sdist * Math.Cos(phi));
            var angle = Math.Atan2(Math.Cos(s.dec) * Math.Sin(s.ra - m.ra), Math.Sin(s.dec) * Math.Cos(m.dec) - Math.Cos(s.dec) * Math.Sin(m.dec) * Math.Cos(s.ra - m.ra));

            return new MoonIllumination
            {
                fraction = (1 + Math.Cos(inc)) / 2,
                phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI,
                angle = angle
            };
        }

        // calculations for moon rise/set times are based on http://www.stargazing.net/kepler/moonrise.html article

        public static MoonTimes GetMoonTimes(DateTime date, double lat, double lng)
        {
            DateTime t = date.Date;

            var hc = 0.133 * rad;
            var h0 = GetMoonPosition(t, lat, lng).altitude - hc;

            int roots;
            double h1, h2, rise = 0, set = 0, a, b, xe, ye = 0, d, x1 = 0, x2 = 0, dx;
            // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
            for (var i = 1; i <= 24; i += 2)
            {
                h1 = GetMoonPosition(t.AddHours(i), lat, lng).altitude - hc;
                h2 = GetMoonPosition(t.AddHours(i + 1), lat, lng).altitude - hc;

                a = (h0 + h2) / 2 - h1;
                b = (h2 - h0) / 2;
                xe = -b / (2 * a);
                ye = (a * xe + b) * xe + h1;
                d = b * b - 4 * a * h1;
                roots = 0;

                if (d >= 0) {
                    dx = Math.Sqrt(d) / (Math.Abs(a) * 2);
                    x1 = xe - dx;
                    x2 = xe + dx;
                    if (Math.Abs(x1) <= 1) roots++;
                    if (Math.Abs(x2) <= 1) roots++;
                    if (x1 < -1) x1 = x2;
                }

                if (roots == 1) 
                {
                    if (h0 < 0) rise = i + x1;
                    else set = i + x1;

                } 
                else if (roots == 2) {
                    rise = i + (ye < 0 ? x2 : x1);
                    set = i + (ye < 0 ? x1 : x2);
                }

                if (rise > 0 && set > 0)
                {
                    break;
                }

                h0 = h2;
            }

            var result = new MoonTimes();

            if (rise > 0)
            {
                result.rise = t.AddHours(rise);
            }

            if (set > 0)
            {
                result.set = t.AddHours(set);
            }

            if (rise <= 0 && set <= 0)
            {
                if (ye > 0)
                {
                    result.alwaysUp = true;
                }
                else
                {
                    result.alwaysDown = true;
                }
            }

            return result;
        }
    }
}