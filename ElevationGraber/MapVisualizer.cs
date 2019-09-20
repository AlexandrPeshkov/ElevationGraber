using ElevationGraber.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ElevationGraber
{
    public class MapVisualizer
    {
        private int maxElevation = 50;
        private const double earthRadius = 6378137.0;

    

        private ColorHeatMap ColorHeatMap { get; set; }

        public MapVisualizer()
        {
            ColorHeatMap = new ColorHeatMap();
        }

        private double DegreeToRadian(double angle) => Math.PI * angle / 180.0;

        private double Secant(double X) => 1 / Math.Cos(X);

        private MapPoint MercatorProjection(AltitudeResponse response)
        {
            var K = Secant(response.Location.Lat);

            var X = (int)(DegreeToRadian(response.Location.Lng) * earthRadius * K ) - 22000000;
            var Y = (int)((Math.Log(Math.Tan(Math.PI / 4 + DegreeToRadian(response.Location.Lat) / 2)) * earthRadius) * K) - 8000000;

            return new MapPoint(X, Y, response.Elevation);
        }

        public void MakeImage(List<List<AltitudeResponse>> elevationMatrix)
        {
            maxElevation = (int)Math.Ceiling(elevationMatrix.Max(l => l.Max(p => p.Elevation)));

            List<List<MapPoint>> data = elevationMatrix
                .Select(l => l
                .Select(p => new MapPoint(p.Location.Lat, p.Location.Lng, p.Elevation))
                .Distinct().ToList())
                .ToList();

            Bitmap bitmap = new Bitmap(data.FirstOrDefault().Count, data.Count);

            foreach(var list in data.ToList())
            {
                list.Reverse();
            }

            data.Reverse();

            for (var i = 0; i < data.Count; i++)
            {
                for (var j = 0; j < data[i].Count; j++)
                {
                    double height = data[i][j].Z;
                    int x = j;
                    int y = i;

                    Color color = CalcElevetionColor(height);
                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmap.Save("ElevationMap.png", ImageFormat.Png);
        }

        private Color CalcElevetionColor(double value)
        {
            return ColorHeatMap.GetColorForValue(value, maxElevation);
        }
    }
}
