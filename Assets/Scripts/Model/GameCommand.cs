using UnityEngine;

public abstract class GameCommand
{
    public abstract GameEvent[] Execute(Game state, GameConfig config);
}

public class PlaceBuildingCommand : GameCommand
{
    int tileId;

    public PlaceBuildingCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(Game state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];

        Tile tile = state.Map.Tiles[tileId];
        int woodCost = config.Building.WoodCost;

        if (tile.IsWater || tile.IsForest || tile.HasBuilding || !state.HasWood(woodCost)) return new GameEvent[0];

        tile.HasBuilding = true;
        state.ChangeWoodCount(-woodCost);

        return new GameEvent[]
        {
            new BuildingPlacedEvent { TileId = tileId },
            new WoodCountChangedEvent { NewCount = state.WoodCount }
        };
    }
}

public class GatherWoodCommand : GameCommand
{
    int tileId;

    public GatherWoodCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(Game state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];

        Tile tile = state.Map.Tiles[tileId];

        if (!tile.IsForest) return new GameEvent[0];

        state.ChangeWoodCount(config.WoodHarvestAmount);

        return new GameEvent[]
        {
            new WoodCountChangedEvent
            {
                NewCount = state.WoodCount
            }
        };
    }
}
