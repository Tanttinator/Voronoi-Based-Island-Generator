using System;
using System.Collections.Generic;
using System.Linq;

public class GameState
{
    public MapState Map { get; private set; }

    List<BuildingState> buildings = new List<BuildingState>();
    List<GatheringZoneState> gatheringZones = new List<GatheringZoneState>();
    Dictionary<int, ClearingZoneState> clearingZones = new Dictionary<int, ClearingZoneState>();
    Dictionary<Resource, int> resources = new Dictionary<Resource, int>();

    public IEnumerable<BuildingState> Buildings => buildings;
    public IEnumerable<GatheringZoneState> GatheringZones => gatheringZones;
    public IEnumerable<(int tileId, ClearingZoneState clearingZone)> ClearingZones => clearingZones.Select(pair => (pair.Key, pair.Value));

    public GameState()
    {
        Map = new MapState();
    }

    public void AddBuilding(Building building) => buildings.Add(new BuildingState(building));
    public void AddGatheringZone() => gatheringZones.Add(new GatheringZoneState());
    public void AddClearingZone(int tileId) => clearingZones.Add(tileId, new ClearingZoneState());
    public void RemoveClearingZone(int tileId) => clearingZones.Remove(tileId);

    public int GetResource(Resource resource) => resources.GetValueOrDefault(resource);
    public void AddResource(ResourceStack stack) => resources[stack.Resource] = GetResource(stack.Resource) + stack.Amount;
    public void RemoveResource(ResourceStack stack) => resources[stack.Resource] = GetResource(stack.Resource) - stack.Amount;
    public void RemoveResources(ResourceStack[] stacks) => Array.ForEach(stacks, RemoveResource);
    public bool HasResource(ResourceStack stack) => GetResource(stack.Resource) >= stack.Amount;
    public bool HasResources(ResourceStack[] stacks) => !stacks.Any(stack => !HasResource(stack));
}
