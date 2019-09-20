namespace ElevationGraber.Models
{
    public class MapPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MapPoint(double x, double y, double z) => (X, Y, Z) = (x, y, z);
    }
}
