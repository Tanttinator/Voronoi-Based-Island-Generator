using StudioIda.Math;
using UnityEngine;

public class GameState
{
    public MapState Map { get; private set; }
    public int WoodCount { get; private set; }

    public GameState()
    {
        Map = new MapState();
    }

    public bool HasWood(int amount) => WoodCount >= amount;
    public void ChangeWoodCount(int amount) => WoodCount += amount;
}
