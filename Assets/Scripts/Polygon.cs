using UnityEngine;

public struct Polygon
{
    public readonly Vector2[] Corners;
    public readonly Vector2 Center;

    public Polygon(params Vector2[] corners)
    {
        Corners = corners;

        Vector2 center = Vector2.zero;
        foreach (Vector2 corner in corners)
            center += corner;
        center /= corners.Length;
        Center = center;
    }
}
