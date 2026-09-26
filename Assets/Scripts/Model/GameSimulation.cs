using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GameSimulation
{
    GameState state;
    GameConfig config;

    public GameSimulation(GameState state, GameConfig config)
    {
        this.state = state;
        this.config = config;
    }

    public GameEvent[] Advance(float dt)
    {
        List<GameEvent> events = new List<GameEvent>();

        foreach (BuildingState building in state.Buildings)
        {
            Job job = building.Type.Job;
            building.AdvanceWorkProgress(dt);
            if (building.WorkProgress >= job.Duration)
            {
                building.SetWorkProgress(building.WorkProgress - job.Duration);
                GameEvent evt = AddResource(job.Output);
                events.Add(evt);
            }
        }

        foreach ((int tileId, GatheringZoneState gatheringZone) in state.GatheringZones)
        {
            gatheringZone.AdvanceProgress(dt);
            if (gatheringZone.Progress >= config.GatheringZoneDuration)
            {
                TileState tile = state.Map.Tiles[tileId];
                Feature feature = tile.Feature;

                gatheringZone.SetProgress(gatheringZone.Progress - config.GatheringZoneDuration);
                ResourceStack output = feature.GatheringOutput[Random.Range(0, feature.GatheringOutput.Length)];
                GameEvent evt = AddResource(output);
                events.Add(evt);
            }
        }

        foreach ((int tileId, ClearingZoneState clearingZone) in state.ClearingZones.ToArray())
        {
            clearingZone.AdvanceProgress(dt);
            if (clearingZone.Progress >= config.ClearingZoneDuration)
            {
                GameEvent evt = AddResource(state.Map.Tiles[tileId].Feature.ClearingOutput);
                events.Add(evt);

                state.RemoveClearingZone(tileId);
                events.Add(new ClearingZoneRemovedEvent { TileId = tileId });

                state.Map.Tiles[tileId].Feature = null;
                events.Add(new FeatureClearedEvent { TileId = tileId });
            }
        }

        foreach ((int tileId, ConstructionZoneState constructionZone) in state.ConstructionZones.ToArray())
        {
            constructionZone.AdvanceProgress(dt);
            if (constructionZone.Progress >= constructionZone.Building.ConstructionDuration)
            {
                state.RemoveConstructionZone(tileId);
                events.Add(new ConstructionZoneRemovedEvent { TileId = tileId });

                state.AddBuilding(constructionZone.Building);
                state.Map.Tiles[tileId].HasBuilding = true;
                events.Add(new BuildingConstructedEvent { TileId = tileId, Building = constructionZone.Building });
            }
        }

        return events.ToArray();
    }

    GameEvent AddResource(ResourceStack stack)
    {
        state.AddResource(stack);
        return new ResourceAmountsChangedEvent() { NewAmounts = new ResourceStack[] { new ResourceStack(stack.Resource, state.GetResource(stack.Resource)) } };
    }
}
