using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractionMode
{
    public GameState State;
    public GameView View;
    public Action<GameCommand> SendCommand;
    public InputProvider Input;

    public virtual void Update()
    {

    }
}

public class DefaultInteractionMode : InteractionMode
{
    public override void Update()
    {
        int tileId = View.GetHoveredTile();
        if (tileId != -1)
            View.Terrain.ShowTileOutline(tileId);
        else
            View.Terrain.HideTileOutline();

        if (Input.Clicked)
        {
            if (tileId == -1) return;

            TileState tile = State.Map.Tiles[tileId];
            if (tile.IsForest)
                SendCommand(new GatherWoodCommand(tileId));
            else
                SendCommand(new PlaceBuildingCommand(tileId));
        }
    }
}
