using StudioIda.Math;
using UnityEngine;

public class Map
{
    public Tile[] Tiles { get; private set; }
    public Rectangle Area { get; private set; }

    public Map(Tile[] tiles, Rectangle area)
    {
        Tiles = tiles;
        Area = area;
    }
}
