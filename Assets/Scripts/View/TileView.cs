using System.Linq;
using StudioIda.Math;
using UnityEngine;

public class TileView
{
    public Polygon Shape { get; private set; }
    public Polygon Surface { get; private set; }
    public GameObject Feature { get; private set; }

    public TileView(Polygon shape)
    {
        Shape = shape;
        Surface = shape;
    }

    public void SetSurface(Polygon surface) => Surface = surface;

    public void AddFeature(GameObject feature) => Feature = feature;
    public void DestroyFeature()
    {
        GameObject.Destroy(Feature);
        Feature = null;
    }

    public Point GetRandomPointOnTile()
    {
        Triangle[] triangulation = Surface.Triangulation;
        float totalArea = triangulation.Select(tri => tri.Area).Aggregate((a, b) => a + b);
        float[] ratios = triangulation.Select(tri => tri.Area / totalArea).ToArray();

        float random = Random.value;
        for (int i = 0; i < triangulation.Length; i++)
        {
            if (random < ratios[i])
            {
                Triangle triangle = triangulation[i];
                return Trigonometry.RandomPointInTriangle(triangle);
            }

            random -= ratios[i];
        }

        return new Point(-1, -1);
    }
}
