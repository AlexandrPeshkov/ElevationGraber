using ElevationGraber.Models;
using System.Threading.Tasks;

namespace ElevationGraber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GeoPoint topLeft = new GeoPoint { Lat = 43.140, Lng = 131.745 };
            GeoPoint bottomRight = new GeoPoint { Lat = 42.948, Lng = 132.047 };

            //GeoPoint topLeft = new GeoPoint { Lat = 43.234794, Lng = 131.675346 };
            //GeoPoint bottomRight = new GeoPoint { Lat = 42.914079, Lng = 32.118520 };

            ElevationGraber elevationGraber = new ElevationGraber(topLeft, bottomRight);
          
            //await elevationGraber.StartGrab();
            //elevationGraber.JsonToHeatMap();
        }
    }
}
