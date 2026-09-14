using System.Collections.Generic;

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
                state.AddResource(job.Output);
                events.Add(new ResourceAmountsChangedEvent() { NewAmounts = new ResourceStack[] { new ResourceStack(job.Output.Resource, state.GetResource(job.Output.Resource)) } });
            }
        }

        return events.ToArray();
    }
}
