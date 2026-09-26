using System.Linq;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class WorldGeneration
{
    public static TileState[] GenerateTiles(Rectangle area, Feature forest, Feature boulders)
    {
        Point[] points = PoissonDisc.SamplePoints(area, 0.5f);

        TileState[] tiles = new TileState[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Point position = points[i];
            bool isWater = Point.DistanceSqr(area.Center, position) > 16;

            bool isForest = !isWater && Random.value < 0.5f;
            bool isBoulders = !isWater && !isForest && Random.value < 0.25f;

            tiles[i] = new TileState(position)
            {
                IsWater = isWater,
                Feature = isForest ? forest : isBoulders ? boulders : null
            };
        }

        return tiles;
    }

    public static int[][] GenerateAdjacencies(TileState[] tiles)
    {
        int[][] adjacencies = new int[tiles.Length][];

        DCEL dcel = Delaunay.Triangulate(tiles.Select(tile => tile.Position).ToArray());

        for (int tile = 0; tile < tiles.Length; tile++)
        {
            Vertex vertex = dcel.Vertices[tile];
            Vertex[] connections = vertex.OutgoingEdges.Select(edge => edge.Next.Origin).ToArray();
            adjacencies[tile] = new int[connections.Length];
            for (int i = 0; i < connections.Length; i++)
            {
                Vertex connection = connections[i];
                int connectionId = dcel.Vertices.IndexOf(connection);
                adjacencies[tile][i] = connectionId;
            }
        }

        return adjacencies;
    }
}
