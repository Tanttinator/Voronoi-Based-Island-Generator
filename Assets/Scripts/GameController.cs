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

        Tile[] tiles = new Tile[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Point position = points[i];
            tiles[i] = new Tile
            {
                Position = position,
                IsWater = Point.DistanceSqr(center, position) > 16
            };
        }

        terrain.GenerateMesh(tiles, area);
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
