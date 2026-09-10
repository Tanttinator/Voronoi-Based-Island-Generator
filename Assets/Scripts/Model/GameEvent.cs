using UnityEngine;

public abstract class GameEvent
{

}

public class BuildingPlacedEvent : GameEvent
{
    public int TileId;
}

public class WoodCountChangedEvent : GameEvent
{
    public int NewCount;
}
