using System.Collections.Generic;
using UnityEngine;

public class TerrainLayerView : MonoBehaviour
{
    MeshFilter filter;

    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> indices = new List<int>();
    List<Color> colors = new List<Color>();

    void Awake()
    {
        filter = GetComponent<MeshFilter>();
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        filter.sharedMesh = mesh;
    }

    public void AddTriangle(Vector3 a, Vector3 b, Vector3 c, Vector2 uva, Vector2 uvb, Vector2 uvc, Color color)
    {
        int startPos = vertices.Count;

        vertices.Add(a);
        vertices.Add(b);
        vertices.Add(c);

        indices.Add(startPos);
        indices.Add(startPos + 1);
        indices.Add(startPos + 2);

        colors.Add(color);
        colors.Add(color);
        colors.Add(color);

        uvs.Add(uva);
        uvs.Add(uvb);
        uvs.Add(uvc);
    }

    public void AddTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
    {
        AddTriangle(a, b, c, Vector2.zero, Vector2.zero, Vector2.zero, color);
    }

    public void AddQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color color)
    {
        AddTriangle(a, b, c, new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), color);
        AddTriangle(a, c, d, new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 0), color);
    }
}
