using System.Collections.Generic;
using System.Linq;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine;

public class TerrainView : MonoBehaviour
{
    [SerializeField] TerrainLayerView groundLayer;
    [SerializeField] TerrainLayerView waterLayer;
    [SerializeField] TerrainLayerView coastLayer;
    [SerializeField] LineRenderer tileOutline;
    [SerializeField] GameObject treePrefab;
    [SerializeField] Color grassColor;
    [SerializeField] Color sandColor;
    [SerializeField] Color coastColor;
    [SerializeField] Color waterColor;
    [SerializeField] float coastWidth;

    public TileView[] Tiles { get; private set; }

    Dictionary<MeshCollider, int> colliderMap = new Dictionary<MeshCollider, int>();

    public void GenerateTerrain(MapState map)
    {
        GenerateTiles(map.Tiles, map.Area);
        GenerateTerrainMesh(map);
        GenerateTerrainFeatures(map.Tiles);
        GenerateTileColliders();
    }

    void GenerateTiles(TileState[] tiles, Rectangle area)
    {
        DCEL dcel = Delaunay.Triangulate(tiles.Select(tile => tile.Position).ToArray());
        Polygon[] cells = Voronoi.FromDelaunay(dcel, Rectangle.Expand(area, 1));

        Tiles = new TileView[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            Tiles[i] = new TileView(cells[i]);
        }
    }

    void GenerateTerrainMesh(MapState map)
    {
        for (int i = 0; i < map.Tiles.Length; i++)
        {
            TileState tile = map.Tiles[i];
            TileView view = Tiles[i];
            if (tile.IsWater)
            {
                Point[] centerPoints = view.Shape.Corners.ToArray();

                int[] adjacencies = map.GetAdjacencies(i);
                for (int edgeId = 0; edgeId < adjacencies.Length; edgeId++)
                {
                    Edge edge = view.Shape.GetEdge(edgeId);
                    TileState neighbor = map.Tiles[adjacencies[edgeId]];
                    if (!neighbor.IsWater)
                    {
                        int leftEdgeId = (edgeId - 1 + adjacencies.Length) % adjacencies.Length;
                        int leftNeighborId = adjacencies[leftEdgeId];
                        bool extendLeft = map.Tiles[leftNeighborId].IsWater;

                        int rightEdgeId = (edgeId + 1) % adjacencies.Length;
                        int rightNeighborId = adjacencies[rightEdgeId];
                        bool extendRight = map.Tiles[rightNeighborId].IsWater;

                        Edge insetEdge = GetInsetEdge(i, edgeId, coastWidth, extendLeft, extendRight, map);
                        CreateCoastWater(edge, insetEdge);

                        centerPoints[leftEdgeId] = insetEdge.A;
                        centerPoints[edgeId] = insetEdge.B;
                    }
                }

                CreateWaterTile(new Polygon(centerPoints));
            }
            else
            {
                Point[] centerPoints = view.Shape.Corners.ToArray();

                int[] adjacencies = map.GetAdjacencies(i);
                for (int edgeId = 0; edgeId < adjacencies.Length; edgeId++)
                {
                    Edge edge = view.Shape.GetEdge(edgeId);
                    TileState neighbor = map.Tiles[adjacencies[edgeId]];
                    if (neighbor.IsWater)
                    {
                        int leftEdgeId = (edgeId - 1 + adjacencies.Length) % adjacencies.Length;
                        int leftNeighborId = adjacencies[leftEdgeId];
                        bool extendLeft = !map.Tiles[leftNeighborId].IsWater;

                        int rightEdgeId = (edgeId + 1) % adjacencies.Length;
                        int rightNeighborId = adjacencies[rightEdgeId];
                        bool extendRight = !map.Tiles[rightNeighborId].IsWater;

                        Edge insetEdge = GetInsetEdge(i, edgeId, 0.1f, extendLeft, extendRight, map);
                        CreateCoastLand(insetEdge, edge);

                        centerPoints[leftEdgeId] = insetEdge.A;
                        centerPoints[edgeId] = insetEdge.B;
                    }
                }

                CreateGroundTile(new Polygon(centerPoints));
            }
        }

        groundLayer.CreateMesh();
        waterLayer.CreateMesh();
        coastLayer.CreateMesh();
    }

    Point GetCornerInset(Edge a, Edge b, float amount)
    {
        float angle = Trigonometry.AngleBetween(a.A, b.A, b.B);
        float distance = amount / Mathf.Sin(angle / 2);
        return b.A + (a.Normal + b.Normal).Normalize() * distance;
    }

    Edge GetInsetEdge(int tileId, int edgeId, float insetAmount, bool extendLeft, bool extendRight, MapState map)
    {
        TileView view = Tiles[tileId];
        int[] adjacencies = map.GetAdjacencies(tileId);
        Edge edge = view.Shape.GetEdge(edgeId);

        int leftEdgeId = (edgeId - 1 + adjacencies.Length) % adjacencies.Length;
        Edge leftEdge = view.Shape.GetEdge(leftEdgeId);
        int leftNeighborId = adjacencies[leftEdgeId];
        if (extendLeft)
        {
            int neighborEdge = map.GetEdgeToTile(leftNeighborId, tileId);
            int neighborAdjacencies = map.GetAdjacencies(leftNeighborId).Length;
            int neighborLeftEdge = (neighborEdge - 1 + neighborAdjacencies) % neighborAdjacencies;
            leftEdge = Tiles[leftNeighborId].Shape.GetEdge(neighborLeftEdge);
        }

        int rightEdgeId = (edgeId + 1) % adjacencies.Length;
        Edge rightEdge = view.Shape.GetEdge(rightEdgeId);
        int rightNeighborId = adjacencies[rightEdgeId];
        if (extendRight)
        {
            int neighborEdge = map.GetEdgeToTile(rightNeighborId, tileId);
            int neighborAdjacencies = map.GetAdjacencies(rightNeighborId).Length;
            int neighborRightEdge = (neighborEdge + 1) % neighborAdjacencies;
            rightEdge = Tiles[rightNeighborId].Shape.GetEdge(neighborRightEdge);
        }

        Point bottomLeft = GetCornerInset(leftEdge, edge, insetAmount);
        Point bottomRight = GetCornerInset(edge, rightEdge, insetAmount);
        return new Edge(bottomLeft, bottomRight);
    }

    void CreateGroundTile(Polygon shape)
    {
        Triangle[] triangulation = EarClipping.TriangulatePolygon(shape);
        for (int j = 0; j < triangulation.Length; j++)
        {
            Triangle triangle = triangulation[j];

            Vector3 a = new Vector3(triangle.a.x, 0, triangle.a.y);
            Vector3 b = new Vector3(triangle.b.x, 0, triangle.b.y);
            Vector3 c = new Vector3(triangle.c.x, 0, triangle.c.y);

            groundLayer.AddTriangle(a, b, c, grassColor);
        }
    }

    void CreateWaterTile(Polygon shape)
    {
        Triangle[] triangulation = EarClipping.TriangulatePolygon(shape);
        for (int j = 0; j < triangulation.Length; j++)
        {
            Triangle triangle = triangulation[j];

            Vector3 a = new Vector3(triangle.a.x, -0.1f, triangle.a.y);
            Vector3 b = new Vector3(triangle.b.x, -0.1f, triangle.b.y);
            Vector3 c = new Vector3(triangle.c.x, -0.1f, triangle.c.y);

            waterLayer.AddTriangle(a, b, c, Color.white);
        }
    }

    void CreateCoastWater(Edge top, Edge bottom)
    {
        Vector3 a = new Vector3(bottom.A.x, -0.1f, bottom.A.y);
        Vector3 b = new Vector3(top.A.x, -0.1f, top.A.y);
        Vector3 c = new Vector3(top.B.x, -0.1f, top.B.y);
        Vector3 d = new Vector3(bottom.B.x, -0.1f, bottom.B.y);

        coastLayer.AddQuad(a, b, c, d, Color.white);
    }

    void CreateCoastLand(Edge top, Edge bottom)
    {
        Vector3 a = new Vector3(bottom.B.x, -0.1f, bottom.B.y);
        Vector3 b = new Vector3(top.B.x, 0f, top.B.y);
        Vector3 c = new Vector3(top.A.x, 0f, top.A.y);
        Vector3 d = new Vector3(bottom.A.x, -0.1f, bottom.A.y);

        groundLayer.AddQuad(a, b, c, d, sandColor);
    }

    void GenerateTerrainFeatures(TileState[] tiles)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].IsForest)
            {
                TileView tile = Tiles[i];

                GameObject forest = new GameObject("Forest");
                forest.transform.SetParent(transform);
                tile.AddFeature(forest);

                for (int j = 0; j < 10; j++)
                {
                    Point position = tile.GetRandomPointOnTile();

                    GameObject tree = Instantiate(treePrefab, forest.transform);
                    tree.transform.position = new Vector3(position.x, 0, position.y);
                    tree.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                    tree.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
                }
            }
        }
    }

    void GenerateTileColliders()
    {
        for (int i = 0; i < Tiles.Length; i++)
        {
            TileView view = Tiles[i];

            Mesh colliderMesh = new Mesh();
            List<Vector3> colliderVertices = new List<Vector3>();
            List<int> colliderIndices = new List<int>();

            for (int j = 0; j < view.Triangulation.Length; j++)
            {
                Triangle triangle = view.Triangulation[j];

                Vector3 a = CellToWorldPoint(triangle.a);
                Vector3 b = CellToWorldPoint(triangle.b);
                Vector3 c = CellToWorldPoint(triangle.c);

                colliderVertices.Add(a);
                colliderVertices.Add(b);
                colliderVertices.Add(c);

                colliderIndices.Add(j * 3);
                colliderIndices.Add(j * 3 + 1);
                colliderIndices.Add(j * 3 + 2);
            }

            colliderMesh.vertices = colliderVertices.ToArray();
            colliderMesh.triangles = colliderIndices.ToArray();

            GameObject colliderGO = new GameObject("Tile Collider");
            colliderGO.transform.SetParent(transform);
            colliderGO.layer = LayerMask.NameToLayer("Tile");
            MeshCollider collider = colliderGO.AddComponent<MeshCollider>();
            collider.sharedMesh = colliderMesh;

            colliderMap[collider] = i;
        }
    }

    Vector3 CellToWorldPoint(Point point)
    {
        return new Vector3(point.x, 0, point.y);
    }

    public int GetCollidedTile(MeshCollider collider)
    {
        return colliderMap[collider];
    }

    public void ShowTileOutline(int tileId)
    {
        TileView view = Tiles[tileId];
        tileOutline.positionCount = view.Shape.Corners.Length;
        tileOutline.SetPositions(view.Shape.Corners.Select(p => new Vector3(p.x, 0.01f, p.y)).ToArray());
    }

    public void HideTileOutline()
    {
        tileOutline.positionCount = 0;
    }
}
