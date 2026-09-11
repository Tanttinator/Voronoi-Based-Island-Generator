using StudioIda.Math;
using UnityEngine;

public class MapState
{
    public TileState[] Tiles { get; private set; }
    public Rectangle Area { get; private set; }

    public void SetTiles(TileState[] tiles) => Tiles = tiles;
    public void SetArea(Rectangle area) => Area = area;
}
