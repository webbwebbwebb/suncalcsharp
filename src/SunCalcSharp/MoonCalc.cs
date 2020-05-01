using System;
using SunCalcSharp.Formulas;

namespace SunCalcSharp
{
    public static class MoonCalc
    {
        /// <summary>
        /// Calculates lunar position for a given location and point in time
        ///
        /// based on http://aa.quae.nl/en/reken/hemelpositie.html formulas
        /// </summary>
        /// <param name="date">time and date calculate for</param>
        /// <param name="latitude">latitude in degrees</param>
        /// <param name="longitude">longitude in degrees</param>
        /// <returns></returns>
        public static MoonPosition GetMoonPosition(DateTime date, double latitude, double longitude)
        {
            var lw = Constants.Rad * -longitude;
            var phi = Constants.Rad * latitude;
            var d = Calendar.ToDays(date);

            var c = Moon.Coordinates(d);
            var H = Position.SiderealTime(d, lw) - c.RightAscension;
            var h = Position.Altitude(H, phi, c.Declination);
            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            var pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.Declination) - Math.Sin(c.Declination) * Math.Cos(H));

            h = h + Moon.AstroRefraction(h); // altitude correction for refraction

            return new MoonPosition
            {
                Azimuth = Position.Azimuth(H, phi, c.Declination),
                Altitude = h,
                Distance = c.Distance,
                ParallacticAngle = pa
            };
        }

        
        /// <summary>
        /// calculations for illumination parameters of the moon,
        /// based on http://idlastro.gsfc.nasa.gov/ftp/pro/astro/mphase.pro formulas and
        /// Chapter 48 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
        /// </summary>
        /// <param name="date">time and date to calculate for</param>
        /// <returns></returns>
        public static MoonIllumination GetMoonIllumination(DateTime date)
        {
            var d = Calendar.ToDays(date);
            var s = Sun.Coordinates(d);
            var m = Moon.Coordinates(d);

            const int sdist = 149598000; // distance from Earth to Sun in km
            var phi = Math.Acos(Math.Sin(s.Declination) * Math.Sin(m.Declination) + Math.Cos(s.Declination) * Math.Cos(m.Declination) * Math.Cos(s.RightAscension - m.RightAscension));
            var inc = Math.Atan2(sdist * Math.Sin(phi), m.Distance - sdist * Math.Cos(phi));
            var angle = Math.Atan2(Math.Cos(s.Declination) * Math.Sin(s.RightAscension - m.RightAscension), Math.Sin(s.Declination) * Math.Cos(m.Declination) - Math.Cos(s.Declination) * Math.Sin(m.Declination) * Math.Cos(s.RightAscension - m.RightAscension));

            return new MoonIllumination
            {
                Fraction = (1 + Math.Cos(inc)) / 2,
                Phase = 0.5 + 0.5 * inc * (angle < 0 ? -1 : 1) / Math.PI,
                Angle = angle
            };
        }

        /// <summary>
        /// Calculations for moon rise/set times are based on http://www.stargazing.net/kepler/moonrise.html
        /// </summary>
        /// <param name="date">date to calculate for</param>
        /// <param name="latitude">latitude in degrees</param>
        /// <param name="longitude">longitude in degrees</param>
        /// <returns></returns>
        public static MoonTimes GetMoonTimes(DateTime date, double latitude, double longitude)
        {
            DateTime t = date.Date;

            var hc = 0.133 * Constants.Rad;
            var h0 = GetMoonPosition(t, latitude, longitude).Altitude - hc;

            int roots;
            double h1, h2, rise = 0, set = 0, a, b, xe, ye = 0, d, x1 = 0, x2 = 0, dx;
            // go in 2-hour chunks, each time seeing if a 3-point quadratic curve crosses zero (which means rise or set)
            for (var i = 1; i <= 24; i += 2)
            {
                h1 = GetMoonPosition(t.AddHours(i), latitude, longitude).Altitude - hc;
                h2 = GetMoonPosition(t.AddHours(i + 1), latitude, longitude).Altitude - hc;

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
                result.Rise = t.AddHours(rise);
            }

            if (set > 0)
            {
                result.Set = t.AddHours(set);
            }

            if (rise <= 0 && set <= 0)
            {
                if (ye > 0)
                {
                    result.AlwaysUp = true;
                }
                else
                {
                    result.AlwaysDown = true;
                }
            }

            return result;
        }
    }
}