using System.Collections.Generic;
using System.Linq;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine;

public class TerrainView : MonoBehaviour
{
    [SerializeField] TerrainLayerView groundLayer;
    [SerializeField] TerrainLayerView waterLayer;
    [SerializeField] LineRenderer tileOutline;
    [SerializeField] GameObject treePrefab;

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
                CreateWaterTile(view);
            else
                CreateGroundTile(view, map.GetAdjacencies(i).Select(tileId => map.Tiles[tileId].IsWater).ToArray());
        }

        groundLayer.CreateMesh();
        waterLayer.CreateMesh();
    }

    void CreateGroundTile(TileView tile, bool[] waterNeighbors)
    {
        for (int j = 0; j < tile.Triangulation.Length; j++)
        {
            Triangle triangle = tile.Triangulation[j];

            Vector3 a = new Vector3(triangle.a.x, 0, triangle.a.y);
            Vector3 b = new Vector3(triangle.b.x, 0, triangle.b.y);
            Vector3 c = new Vector3(triangle.c.x, 0, triangle.c.y);

            groundLayer.AddTriangle(a, b, c, Color.green);
        }

        for (int i = 0; i < waterNeighbors.Length; i++)
        {
            if (waterNeighbors[i]) CreateCoast(tile, i);
        }
    }

    void CreateCoast(TileView tile, int edge)
    {
        (Point a, Point b) = tile.Shape.GetEdge(edge);

        Vector3 va = new Vector3(b.x, -0.1f, b.y);
        Vector3 vb = new Vector3(b.x, 0, b.y);
        Vector3 vc = new Vector3(a.x, 0, a.y);
        Vector3 vd = new Vector3(a.x, -0.1f, a.y);

        groundLayer.AddQuad(va, vb, vc, vd, Color.beige);
    }

    void CreateWaterTile(TileView tile)
    {
        for (int j = 0; j < tile.Triangulation.Length; j++)
        {
            Triangle triangle = tile.Triangulation[j];

            Vector3 a = new Vector3(triangle.a.x, -0.1f, triangle.a.y);
            Vector3 b = new Vector3(triangle.b.x, -0.1f, triangle.b.y);
            Vector3 c = new Vector3(triangle.c.x, -0.1f, triangle.c.y);

            waterLayer.AddTriangle(a, b, c, Color.blue);
        }
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
