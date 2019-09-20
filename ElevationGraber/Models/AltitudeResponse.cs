using ElevationGraber.Models;

namespace ElevationGraber
{
    public class AltitudeResponse
    {
        public double Elevation { get; set; }

        public GeoPoint Location { get; set; }

        public double Resolution { get; set; }
    }
}
