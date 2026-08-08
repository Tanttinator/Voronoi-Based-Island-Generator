using StudioIda.Math;
using UnityEngine;

public class TileView
{
    public Polygon Shape { get; private set; }
    public Triangle[] Triangulation { get; private set; }

    public TileView(Polygon shape)
    {
        Shape = shape;
        Triangulation = EarClipping.TriangulatePolygon(shape);
    }
}
