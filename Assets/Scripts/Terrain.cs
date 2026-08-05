using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    MeshFilter filter;

    public void GenerateMesh(Polygon[] cells)
    {
        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> colors = new List<Color>();

        foreach (Polygon cell in cells)
        {
            Color color = Random.ColorHSV(0f, 1f);
            Vector3 center = CellToWorldPoint(cell.Center);
            for (int i = 0; i < cell.Corners.Length; i++)
            {
                Vector3 a = CellToWorldPoint(cell.Corners[i]);
                Vector3 b = CellToWorldPoint(cell.Corners[(i + 1) % cell.Corners.Length]);

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
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }

    Vector3 CellToWorldPoint(Vector2 point)
    {
        return new Vector3(point.x, 0, point.y);
    }

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }
}
