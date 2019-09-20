using ElevationGraber.Models;
using System.Collections.Generic;

namespace ElevationGraber.Extensions
{
    public static class MapPointExtensions
    {
        public static MapPoint FindClosestPeekPoint(List<MapPoint> dataset, MapPoint point, PointSearchDirection searchDirection)
        {
            switch (searchDirection)
            {
                default:
                    {
                        return null;
                    }
            }
        }

        public enum PointSearchDirection
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
