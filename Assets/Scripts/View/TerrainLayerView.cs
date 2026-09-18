using System.Collections.Generic;
using UnityEngine;

public class TerrainLayerView : MonoBehaviour
{
    MeshFilter filter;

    List<Vector3> vertices = new List<Vector3>();
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
        mesh.triangles = indices.ToArray();
        mesh.colors = colors.ToArray();
        filter.sharedMesh = mesh;
    }

    public void AddTriangle(Vector3 a, Vector3 b, Vector3 c, Color color)
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
    }
}
