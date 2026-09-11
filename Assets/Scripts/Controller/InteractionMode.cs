using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractionMode
{
    public GameState State;
    public GameView View;
    public InputProvider Input;

    public virtual GameCommand Update()
    {
        return GameCommand.None;
    }
}

public class DefaultInteractionMode : InteractionMode
{
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
            if (tile.IsForest)
                return new GatherWoodCommand(tileId);
            else
                return new PlaceBuildingCommand(tileId);
        }

        return GameCommand.None;
    }
}
