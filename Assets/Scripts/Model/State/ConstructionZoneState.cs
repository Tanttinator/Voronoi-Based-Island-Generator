using UnityEngine;

public class ConstructionZoneState
{
    public Building Building { get; private set; }
    public float Progress { get; private set; }

    public ConstructionZoneState(Building building)
    {
        Building = building;
    }

    public void SetProgress(float progress) => Progress = progress;
    public void AdvanceProgress(float amount) => Progress += amount;
}
