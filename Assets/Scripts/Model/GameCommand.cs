using UnityEngine;

public abstract class GameCommand
{
    public abstract GameEvent[] Execute(Game state);
}

public class PlaceBuildingCommand : GameCommand
{
    int tileId;

    public PlaceBuildingCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(Game state)
    {
        if (tileId == -1) return new GameEvent[0];

        Tile tile = state.Map.Tiles[tileId];

        if (tile.IsWater || tile.HasBuilding) return new GameEvent[0];

        tile.HasBuilding = true;

        return new GameEvent[]
        {
            new BuildingPlacedEvent
            {
                TileId = tileId
            }
        };
    }
}
