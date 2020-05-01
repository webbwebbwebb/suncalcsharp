using System;

namespace SunCalcSharp
{
    public static class MoonCalc
    {
        // moon calculations, based on http://aa.quae.nl/en/reken/hemelpositie.html formulas
        public static MoonPosition GetMoonPosition(DateTime date, double lat, double lng)
        {
            var lw = Constants.rad * -lng;
            var phi = Constants.rad * lat;
            var d = Calendar.toDays(date);

            var c = MoonCalculations.moonCoords(d);
            var H = PositionCalculations.siderealTime(d, lw) - c.ra;
            var h = PositionCalculations.altitude(H, phi, c.dec);
            // formula 14.1 of "Astronomical Algorithms" 2nd edition by Jean Meeus (Willmann-Bell, Richmond) 1998.
            var pa = Math.Atan2(Math.Sin(H), Math.Tan(phi) * Math.Cos(c.dec) - Math.Sin(c.dec) * Math.Cos(H));

            h = h + MoonCalculations.astroRefraction(h); // altitude correction for refraction

            return new MoonPosition
            {
                azimuth = PositionCalculations.azimuth(H, phi, c.dec),
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
            var d = Calendar.toDays(date);
            var s = SunCalculations.sunCoords(d);
            var m = MoonCalculations.moonCoords(d);

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

            var hc = 0.133 * Constants.rad;
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