using System.Collections.Generic;
using UnityEngine;
using StudioIda.Math;
using System.Linq;
using StudioIda.Graphs;

public class Terrain : MonoBehaviour
{
    [SerializeField] MeshCollider tileCollider;
    [SerializeField] LineRenderer tileOutline;

    Dictionary<Tile, TileView> tilesMap = new Dictionary<Tile, TileView>();
    Dictionary<MeshCollider, Tile> colliderMap = new Dictionary<MeshCollider, Tile>();

    MeshFilter filter;

    public void GenerateMesh(Tile[] tiles, Rectangle area)
    {
        DCEL dcel = Delaunay.Triangulate(tiles.Select(tile => tile.Position).ToArray());
        Polygon[] cells = Voronoi.FromDelaunay(dcel, area);

        for (int i = 0; i < tiles.Length; i++)
        {
            tilesMap[tiles[i]] = new TileView(cells[i]);
        }

        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        foreach (Tile tile in tiles)
        {
            TileView view = tilesMap[tile];

            Mesh colliderMesh = new Mesh();
            List<Vector3> colliderVertices = new List<Vector3>();
            List<int> colliderIndices = new List<int>();

            Color color = tile.IsWater ? Color.blue : Color.green;

            for (int i = 0; i < view.Triangulation.Length; i++)
            {
                Triangle triangle = view.Triangulation[i];
                int startIndex = vertices.Count;

                Vector3 a = CellToWorldPoint(triangle.a);
                Vector3 b = CellToWorldPoint(triangle.b);
                Vector3 c = CellToWorldPoint(triangle.c);

                vertices.Add(a);
                vertices.Add(b);
                vertices.Add(c);

                indices.Add(startIndex);
                indices.Add(startIndex + 1);
                indices.Add(startIndex + 2);

                colors.Add(color);
                colors.Add(color);
                colors.Add(color);

                colliderVertices.Add(a);
                colliderVertices.Add(b);
                colliderVertices.Add(c);

                colliderIndices.Add(i * 3);
                colliderIndices.Add(i * 3 + 1);
                colliderIndices.Add(i * 3 + 2);
            }

            colliderMesh.vertices = colliderVertices.ToArray();
            colliderMesh.triangles = colliderIndices.ToArray();

            MeshCollider collider = Instantiate(tileCollider, transform);
            collider.sharedMesh = colliderMesh;

            colliderMap[collider] = tile;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }

    public Tile GetCollidedTile(MeshCollider collider)
    {
        return colliderMap[collider];
    }

    public void ShowTileOutline(Tile tile)
    {
        TileView view = tilesMap[tile];
        tileOutline.positionCount = view.Shape.Corners.Length;
        tileOutline.SetPositions(view.Shape.Corners.Select(p => new Vector3(p.x, 0.01f, p.y)).ToArray());
    }

    public void HideTileOutline()
    {
        tileOutline.positionCount = 0;
    }

    Vector3 CellToWorldPoint(Point point)
    {
        return new Vector3(point.x, 0, point.y);
    }

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }
}
