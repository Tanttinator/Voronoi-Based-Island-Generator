using System.Collections.Generic;
using StudioIda.Math;
using UnityEngine;

public class GameState
{
    public MapState Map { get; private set; }
    public List<BuildingState> Buildings { get; private set; } = new List<BuildingState>();
    public int WoodCount { get; private set; }

    public GameState()
    {
        Map = new MapState();
    }

    public void AddBuilding() => Buildings.Add(new BuildingState());
    public bool HasWood(int amount) => WoodCount >= amount;
    public void ChangeWoodCount(int amount) => WoodCount += amount;
}
