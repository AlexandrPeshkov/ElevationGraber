namespace ElevationGraber.Models
{
    public class MapPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public MapPoint(double x = 0, double y = 0, double z = 0) => (X, Y, Z) = (x, y, z);
    }
}
