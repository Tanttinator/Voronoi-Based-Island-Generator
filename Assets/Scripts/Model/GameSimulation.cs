using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        foreach (GatheringZoneState gatheringZone in state.GatheringZones)
        {
            gatheringZone.AdvanceProgress(dt);
            if (gatheringZone.Progress >= config.GatheringZoneDuration)
            {
                gatheringZone.SetProgress(gatheringZone.Progress - config.GatheringZoneDuration);
                ResourceStack output = config.GatheringZoneOutput[Random.Range(0, config.GatheringZoneOutput.Length)];
                GameEvent evt = AddResource(output);
                events.Add(evt);
            }
        }

        foreach ((int tileId, ClearingZoneState clearingZone) in state.ClearingZones.ToArray())
        {
            clearingZone.AdvanceProgress(dt);
            if (clearingZone.Progress >= config.ClearingZoneDuration)
            {
                state.RemoveClearingZone(tileId);
                events.Add(new ClearingZoneRemovedEvent { TileId = tileId });

                state.Map.Tiles[tileId].IsForest = false;
                events.Add(new ForestClearedEvent { TileId = tileId });

                GameEvent evt = AddResource(config.ClearingZoneOutput);
                events.Add(evt);
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
