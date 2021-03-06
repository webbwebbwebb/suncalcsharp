# suncalcsharp
.NET library for calculating sun/moon positions and phases.

Ported from the [SunCalc](https://github.com/mourner/suncalc) JavaScript library.

### Getting started

1, Install SunCalcSharp from [NuGet](https://www.nuget.org/packages/SunCalcSharp):
```
Install-Package SunCalcSharp
```

2, Include SunCalcSharp in the list of namespaces referenced by your class
```csharp
using SunCalcSharp;
```

3, Call the static methods on SunCalc (for solar calculations) or MoonCalc (for lunar calculations):
```csharp
// get today's sun event times for Milton Keynes
var times = SunCalc.GetTimes(DateTime.UtcNow, 52.0406, -0.7594);
var sunrise = times.Sunrise;
var sunset = times.Sunset;

// get position of the sun (azimuth and altitude) at sunrise
var positionAtSunrise = SunCalc.GetPosition(times.Sunrise, 52.0406, -0.7594);
var solarAzimuthAtSunrise = positionAtSunrise.Azimuth;
var solarAltitudeAtSunrise = positionAtSunrise.Altitude;

// get solar azimuth in degrees
var azimuthInDegrees = positionAtSunrise.Azimuth * 180 / Math.PI;

```

