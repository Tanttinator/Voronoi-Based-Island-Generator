using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameCommand
{
    public static readonly GameCommand None = new GameCommand();

    public virtual GameEvent[] Execute(GameState state, GameConfig config)
    {
        return new GameEvent[0];
    }
}

public class PlaceConstructionZoneCommand : GameCommand
{
    Building building;
    int tileId;

    public PlaceConstructionZoneCommand(Building building, int tileId)
    {
        this.building = building;
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];
        TileState tile = state.Map.Tiles[tileId];
        if (tile.IsWater || tile.IsForest || tile.HasBuilding) return new GameEvent[0];

        ResourceStack[] cost = building.Cost;
        if (!state.HasResources(cost)) return new GameEvent[0];

        state.AddConstructionZone(tileId, building);
        state.RemoveResources(cost);

        return new GameEvent[]
        {
            new ConstructionZonePlacedEvent { TileId = tileId },
            new ResourceAmountsChangedEvent { NewAmounts = cost.Select(stack => stack.Resource).Select(resource => new ResourceStack(resource, state.GetResource(resource))).ToArray() }
        };
    }
}

public class PlaceGatheringZoneCommand : GameCommand
{
    int tileId;

    public PlaceGatheringZoneCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];

        state.AddGatheringZone();

        return new GameEvent[]
        {
            new GatheringZonePlacedEvent { TileId = tileId }
        };
    }
}

public class PlaceClearingZoneCommand : GameCommand
{
    int tileId;

    public PlaceClearingZoneCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];

        TileState tile = state.Map.Tiles[tileId];

        if (!tile.IsForest) return new GameEvent[0];

        state.AddClearingZone(tileId);

        return new GameEvent[]
        {
            new ClearingZonePlacedEvent { TileId = tileId }
        };
    }
}
