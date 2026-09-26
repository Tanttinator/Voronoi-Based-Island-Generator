using StudioIda.Math;
using UnityEngine;

public class TileState
{
    public Point Position { get; private set; }
    public bool IsWater;
    public Feature Feature;
    public bool HasBuilding;

    public bool HasFeature => Feature != null;

    public TileState(Point position)
    {
        Position = position;
    }
}
