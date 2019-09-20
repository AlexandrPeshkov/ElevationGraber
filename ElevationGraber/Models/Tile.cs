using System.Collections.Generic;

namespace ElevationGraber.Models
{
    public class Tile
    {
        public int LevelOfDetails { get; set; }

        public int TileX { get; set; }

        public int TileY { get; set; }

        public List<MapPoint> Points { get; set; }

        public Tile(int levelOfDetails, int tileX, int tileY)
        {
            LevelOfDetails = levelOfDetails;
            TileX = tileX;
            TileY = tileY;
            Points = new List<MapPoint>();
        }
    }
}
