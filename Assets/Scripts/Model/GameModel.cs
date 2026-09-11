using StudioIda.Math;
using UnityEngine;

public class GameModel
{
    public GameState State { get; private set; }

    public GameModel()
    {
        State = new GameState();
    }
}
