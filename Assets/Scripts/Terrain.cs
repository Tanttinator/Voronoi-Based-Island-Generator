using System.Collections.Generic;
using UnityEngine;
using StudioIda.Math;
using System.Linq;

public class Terrain : MonoBehaviour
{
    [SerializeField] MeshCollider tileCollider;
    [SerializeField] LineRenderer tileOutline;

    Dictionary<MeshCollider, Tile> colliderMap = new Dictionary<MeshCollider, Tile>();

    MeshFilter filter;

    public void GenerateMesh(Tile[] tiles)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        foreach (Tile tile in tiles)
        {
            Mesh colliderMesh = new Mesh();
            List<Vector3> colliderVertices = new List<Vector3>();
            List<int> colliderIndices = new List<int>();

            Polygon polygon = tile.Shape;
            Color color = tile.IsWater ? Color.blue : Color.green;
            Vector3 center = CellToWorldPoint(polygon.Center);
            for (int i = 0; i < polygon.Corners.Length; i++)
            {
                Vector3 a = CellToWorldPoint(polygon.Corners[i]);
                Vector3 b = CellToWorldPoint(polygon.Corners[(i + 1) % polygon.Corners.Length]);

                int startIndex = vertices.Count;
                vertices.Add(center);
                vertices.Add(a);
                vertices.Add(b);

                indices.Add(startIndex);
                indices.Add(startIndex + 1);
                indices.Add(startIndex + 2);

                colors.Add(color);
                colors.Add(color);
                colors.Add(color);

                colliderVertices.Add(center);
                colliderVertices.Add(a);
                colliderVertices.Add(b);

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
        tileOutline.positionCount = tile.Shape.Corners.Length;
        tileOutline.SetPositions(tile.Shape.Corners.Select(p => new Vector3(p.x, 0.01f, p.y)).ToArray());
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
