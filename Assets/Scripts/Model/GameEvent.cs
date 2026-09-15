using UnityEngine;

public abstract class GameEvent
{

}

public class BuildingPlacedEvent : GameEvent
{
    public Building Building;
    public int TileId;
}

public class ResourceAmountsChangedEvent : GameEvent
{
    public ResourceStack[] NewAmounts;
}

public class GatheringZonePlacedEvent : GameEvent
{
    public int TileId;
}
