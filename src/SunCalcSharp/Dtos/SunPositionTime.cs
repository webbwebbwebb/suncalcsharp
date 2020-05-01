namespace SunCalcSharp.Dtos
{
    internal class SunPositionTime
    {
        public SunPositionTime(double angle, string riseName, string setName)
        {
            this.Angle = angle;
            this.RiseName = riseName;
            this.SetName = setName;
        }

        public double Angle;
        public string RiseName;
        public string SetName;
    }
}