namespace SunCalcSharp
{
    public class SunPositionTime
    {
        public SunPositionTime(double angle, string riseName, string setName)
        {
            this.angle = angle;
            this.riseName = riseName;
            this.setName = setName;
        }

        public double angle;
        public string riseName;
        public string setName;
    }
}