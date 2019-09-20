using ElevationGraber.Models;
using System.Threading.Tasks;

namespace ElevationGraber
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //GeoPoint topLeft = new GeoPoint { Lat = 43.140, Lng = 131.745 };
            //GeoPoint bottomRight = new GeoPoint { Lat = 42.948, Lng = 132.047 };

            //ElevationGraber elevationGraber = new ElevationGraber(topLeft, bottomRight);
            //await elevationGraber.StartGrab();
            //elevationGraber.JsonToHeatMap();

            TileMaker tileMaker = new TileMaker();
            //tileMaker.MakeTileDataSet(14, 15, 16, 17, 18);
            tileMaker.MakeTileDataSet(1);
        }
    }
}
