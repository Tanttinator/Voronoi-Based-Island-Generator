using UnityEngine;

public abstract class GameEvent
{

}

public class BuildingConstructedEvent : GameEvent
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

public class FeatureClearedEvent : GameEvent
{
    public int TileId;
}

public class ClearingZonePlacedEvent : GameEvent
{
    public int TileId;
}

public class ClearingZoneRemovedEvent : GameEvent
{
    public int TileId;
}

public class ConstructionZonePlacedEvent : GameEvent
{
    public int TileId;
}

public class ConstructionZoneRemovedEvent : GameEvent
{
    public int TileId;
}
