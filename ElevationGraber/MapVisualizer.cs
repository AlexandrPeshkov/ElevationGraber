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
        private string pngExt = ".png";
        private ColorHeatMap ColorHeatMap { get; set; }

        public MapVisualizer()
        {
            ColorHeatMap = new ColorHeatMap();
        }
        public void MakeHeatImagePng(List<MapPoint> data, int width = TileSystem.tileSize, int height = TileSystem.tileSize, string fileName = "HeatImage")
        {
            int maxElevation = (int)Math.Ceiling(data.Max(p => p.Z));

            var trimMatrix = data.GroupBy(p => p.Y).Select(g => g.OrderBy(p => p.X).ToList()).ToList();

            if (trimMatrix.Any() && trimMatrix.FirstOrDefault() != null)
            {
                var w = trimMatrix.Max(l => l.Count);
                var h = trimMatrix.Count;
                Bitmap smallBitmap = new Bitmap(w, h);

                for (var i = 0; i < trimMatrix.Count; i++)
                {
                    for (var j = 0; j < trimMatrix[i].Count; j++)
                    {
                        Color color = ColorHeatMap.GetColorForValue(trimMatrix[i][j].Z, maxElevation);
                        smallBitmap.SetPixel(j, i, color);
                    }
                }

                Bitmap result = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(result))
                {
                    SolidBrush black = new SolidBrush(Color.Black);
                    g.FillRectangle(black, new Rectangle(0, 0, width, height));

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    g.DrawImage(smallBitmap, 0, 0, width, height);
                }

                //for (var i = 0; i < width; i++)
                //{
                //    Color color = result.GetPixel(i, height - 1);
                //    result.SetPixel(i, height - 1, color);
                //}

                //for (var i = 0; i < height; i++)
                //{
                //    Color color = result.GetPixel(width - 1, i);
                //    result.SetPixel(width - 1, i, color);
                //}

                result.Save($"{fileName}{pngExt}", ImageFormat.Png);
            }
        }
    }
}
