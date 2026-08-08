using StudioIda.Math;
using UnityEngine;

public class Game
{
    public Map Map { get; private set; }

    public Game(Tile[] tiles, Rectangle area)
    {
        Map = new Map(tiles, area);
    }
}
