using StudioIda.Math;
using UnityEngine;

public class Game
{
    public Map Map { get; private set; }
    public int WoodCount { get; private set; }

    public Game(Tile[] tiles, Rectangle area)
    {
        Map = new Map(tiles, area);
    }

    public bool HasWood(int amount) => WoodCount >= amount;
    public void ChangeWoodCount(int amount) => WoodCount += amount;
}
