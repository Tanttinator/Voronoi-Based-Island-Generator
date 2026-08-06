using UnityEngine;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject cellMarker;
    [SerializeField] LineRenderer edgeMarker;
    [SerializeField] Terrain terrain;

    void GenerateCells()
    {
        float minX = -5;
        float minY = -5;
        float maxX = 5;
        float maxY = 5;
        Rectangle area = new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
        Point center = new Point(minX + maxX, minY + maxY);

        Point[] points = PoissonDisc.SamplePoints(area, 0.5f);
        DCEL dcel = Delaunay.Triangulate(points);
        Polygon[] cells = Voronoi.FromDelaunay(dcel, area);

        Tile[] tiles = new Tile[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            Polygon polygon = cells[i];
            tiles[i] = new Tile
            {
                IsWater = Point.DistanceSqr(center, polygon.Center) > 16,
                Shape = polygon
            };
        }

        terrain.GenerateMesh(tiles);
    }

    void Start()
    {
        terrain.HideTileOutline();
        GenerateCells();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = terrain.GetCollidedTile((MeshCollider)hit.collider);
            terrain.ShowTileOutline(tile);
        }
        else
        {
            terrain.HideTileOutline();
        }
    }
}
