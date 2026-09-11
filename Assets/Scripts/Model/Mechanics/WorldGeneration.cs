using StudioIda.Math;
using UnityEngine;

public static class WorldGeneration
{
    public static TileState[] GenerateTiles(Rectangle area)
    {
        Point[] points = PoissonDisc.SamplePoints(area, 0.5f);

        TileState[] tiles = new TileState[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Point position = points[i];
            bool isWater = Point.DistanceSqr(area.Center, position) > 16;
            bool isForest = !isWater && Random.value < 0.5f;
            tiles[i] = new TileState
            {
                Position = position,
                IsWater = isWater,
                IsForest = isForest
            };
        }

        return tiles;
    }
}
