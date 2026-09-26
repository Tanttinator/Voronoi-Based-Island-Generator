using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameCommand
{
    public static readonly GameCommand None = new GameCommand();

    public virtual IEnumerable<GameEvent> Execute(GameState state, GameConfig config)
    {
        yield break;
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

    public override IEnumerable<GameEvent> Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) yield break;
        TileState tile = state.Map.Tiles[tileId];
        if (tile.IsWater || tile.HasFeature || tile.HasBuilding) yield break;

        ResourceStack[] cost = building.Cost;
        if (!state.HasResources(cost)) yield break;

        state.AddConstructionZone(tileId, building);
        yield return new ConstructionZonePlacedEvent { TileId = tileId };

        state.RemoveResources(cost);
        yield return new ResourceAmountsChangedEvent { NewAmounts = cost.Select(stack => stack.Resource).Select(resource => new ResourceStack(resource, state.GetResource(resource))).ToArray() };
    }
}

public class PlaceGatheringZoneCommand : GameCommand
{
    int tileId;

    public PlaceGatheringZoneCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override IEnumerable<GameEvent> Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) yield break;

        state.AddGatheringZone(tileId);
        yield return new GatheringZonePlacedEvent { TileId = tileId };
    }
}

public class PlaceClearingZoneCommand : GameCommand
{
    int tileId;

    public PlaceClearingZoneCommand(int tileId)
    {
        this.tileId = tileId;
    }

    public override IEnumerable<GameEvent> Execute(GameState state, GameConfig config)
    {
        if (tileId == -1) yield break;

        TileState tile = state.Map.Tiles[tileId];

        if (!tile.HasFeature) yield break;

        state.AddClearingZone(tileId);
        yield return new ClearingZonePlacedEvent { TileId = tileId };
    }
}
