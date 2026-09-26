using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractionMode
{
    public GameState State;
    public GameView View;
    public InputProvider Input;
    public GameConfig Config;

    public virtual void Start()
    {

    }

    public virtual GameCommand Update()
    {
        return GameCommand.None;
    }

    public virtual void End()
    {

    }
}

public class DefaultInteractionMode : InteractionMode
{
    Building selectedBuilding = null;

    public override void Start()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected += OnBuildingSelected;
    }

    public override GameCommand Update()
    {
        int tileId = View.GetHoveredTile();
        if (tileId != -1)
            View.Terrain.ShowTileOutline(tileId);
        else
            View.Terrain.HideTileOutline();

        if (Input.Clicked)
        {
            if (tileId == -1) return GameCommand.None;

            TileState tile = State.Map.Tiles[tileId];
            if (tile.HasFeature)
                return new PlaceGatheringZoneCommand(tileId);
            else if (selectedBuilding != null && !tile.IsWater)
                return new PlaceConstructionZoneCommand(selectedBuilding, tileId);
        }

        return GameCommand.None;
    }

    public override void End()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected -= OnBuildingSelected;
    }

    void OnBuildingSelected(Building building)
    {
        selectedBuilding = building;
    }
}
