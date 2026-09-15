using System.Linq;
using StudioIda.Math;
using UnityEngine;

public class TileView
{
    public Polygon Shape { get; private set; }
    public Triangle[] Triangulation { get; private set; }
    public GameObject Feature { get; private set; }

    public TileView(Polygon shape)
    {
        Shape = shape;
        Triangulation = EarClipping.TriangulatePolygon(shape);
    }

    public void AddFeature(GameObject feature) => Feature = feature;
    public void DestroyFeature()
    {
        GameObject.Destroy(Feature);
        Feature = null;
    }

    public Point GetRandomPointOnTile()
    {
        float totalArea = Triangulation.Select(tri => tri.Area).Aggregate((a, b) => a + b);
        float[] ratios = Triangulation.Select(tri => tri.Area / totalArea).ToArray();

        float random = Random.value;
        for (int i = 0; i < Triangulation.Length; i++)
        {
            if (random < ratios[i])
            {
                Triangle triangle = Triangulation[i];
                return Trigonometry.RandomPointInTriangle(triangle);
            }

            random -= ratios[i];
        }

        return new Point(-1, -1);
    }
}
