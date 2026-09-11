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
            building.AdvanceWorkProgress(dt);
            if (building.WorkProgress >= config.Building.WorkDuration)
            {
                building.SetWorkProgress(building.WorkProgress - config.Building.WorkDuration);
                state.ChangeWoodCount(config.WoodHarvestAmount);
                events.Add(new WoodCountChangedEvent() { NewCount = state.WoodCount });
            }
        }

        return events.ToArray();
    }
}
