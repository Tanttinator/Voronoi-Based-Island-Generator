using System.Linq;
using UnityEngine;

public class GameCommand
{
    public static readonly GameCommand None = new GameCommand();

    public virtual GameEvent[] Execute(GameState state, GameConfig config)
    {
        return new GameEvent[0];
    }
}

public class PlaceBuildingCommand : GameCommand
{
    Building building;
    int tileId;

    public PlaceBuildingCommand(Building building, int tileId)
    {
        this.building = building;
        this.tileId = tileId;
    }

    public override GameEvent[] Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) return new GameEvent[0];

        TileState tile = state.Map.Tiles[tileId];
        ResourceStack[] cost = building.Cost;

        if (tile.IsWater || tile.IsForest || tile.HasBuilding || !state.HasResources(cost)) return new GameEvent[0];

        tile.HasBuilding = true;
        state.AddBuilding(building);
        state.RemoveResources(cost);

        return new GameEvent[]
        {
            new BuildingPlacedEvent { Building = building, TileId = tileId },
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
