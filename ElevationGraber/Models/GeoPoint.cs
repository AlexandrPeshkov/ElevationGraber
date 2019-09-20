using System.Globalization;

namespace ElevationGraber.Models
{
    public class GeoPoint
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public override string ToString()
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return $"{Lat.ToString(nfi)}, {Lng.ToString(nfi)}";
        }
    }
}
