using UnityEngine;

public abstract class GameEvent
{

}

public class BuildingPlacedEvent : GameEvent
{
    public int TileId;
}

public class ResourceAmountsChangedEvent : GameEvent
{
    public ResourceStack[] NewAmounts;
}
