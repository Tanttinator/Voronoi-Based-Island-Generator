using StudioIda.Math;
using UnityEngine;

public class GameModel
{
    public GameState State { get; private set; }

    GameConfig config;
    GameSimulation simulation;

    public GameModel(GameConfig config)
    {
        State = new GameState();
        this.config = config;
        simulation = new GameSimulation(State, config);
    }

    public GameEvent[] Simulate(float dt)
    {
        return simulation.Advance(dt);
    }
}
