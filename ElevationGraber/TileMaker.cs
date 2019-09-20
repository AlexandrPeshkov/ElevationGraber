using ElevationGraber.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElevationGraber
{
    public class TileMaker
    {
        private JsonSerializerSettings JsonConverterSettings { get; set; }

        private MapVisualizer MapVisualizer { get; set; }

        public TileMaker()
        {
            JsonConverterSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
            };

            MapVisualizer = new MapVisualizer();
        }
        private List<List<AltitudeResponse>> ReadDataFromJsonFile(string path = "AltitudeData.json")
        {
            string json = File.ReadAllText(path);
            List<List<AltitudeResponse>> data = JsonConvert.DeserializeObject<List<List<AltitudeResponse>>>(json, JsonConverterSettings);
            data = data.Where(l => l != null).Select(l => l.Where(p => p != null).ToList()).ToList();
            return data;
        }
        public void MakeTileDataSet(params int[] levelsOfDetails)
        {
            List<List<AltitudeResponse>> dataset = ReadDataFromJsonFile();
            foreach (var level in levelsOfDetails)
            {
                List<Tile> tileMatrix = MakeTileMatrix(level, dataset);
                WriteTileMatrixToPng(tileMatrix, level);
            }
        }
        private List<Tile> MakeTileMatrix(int levelOfDetail, List<List<AltitudeResponse>> altitudes)
        {
            List<Tile> tileMatrix = new List<Tile>();

            foreach (var line in altitudes)
            {
                foreach (var point in line)
                {
                    TileSystem.LatLongToPixelXY(point.Location.Lat, point.Location.Lng, levelOfDetail, out var pixelX, out var pixelY);
                    TileSystem.PixelXYToTileXY(pixelX, pixelY, out var tileX, out var tileY);
                    TileSystem.MapPixelXYToTileInnerPixelXY(pixelX, pixelY, out var tileInnerX, out var tileInnerY);

                    MapPoint mapPoint = new MapPoint(tileInnerX, tileInnerY, point.Elevation);

                    if (mapPoint.Z > 0)
                    {
                        Tile tile = tileMatrix.FirstOrDefault(t => t.TileX == tileX && t.TileY == tileY);
                        if (tile == null)
                        {
                            tile = new Tile(levelOfDetail, tileX, tileY);
                            tileMatrix.Add(tile);
                        }
                        tile.Points.Add(mapPoint);
                    }
                }
            }
            return tileMatrix;
        }

        private void WriteTileMatrixToPng(List<Tile> tileMatrix, int levelOfDetails)
        {
            string tilesDirectory = "Tiles";
            string levelDirectory = $"{tilesDirectory}/{levelOfDetails}";

            if (!Directory.Exists(tilesDirectory))
            {
                Directory.CreateDirectory(tilesDirectory);
            }

            if (!Directory.Exists(levelDirectory))
            {
                Directory.CreateDirectory(levelDirectory);
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(levelDirectory);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }

            for (var i = 0; i < tileMatrix.Count; i++)
            {
                WriteTileToPng(tileMatrix[i], levelDirectory);
            }
        }
        private void WriteTileToPng(Tile tile, string path)
        {
            string tilePath = $"{path}/tile_X{tile.TileX}_Y{tile.TileY}";

            MapVisualizer.MakeHeatImagePng(tile.Points, TileSystem.tileSize, TileSystem.tileSize, tilePath);
        }
    }
}
