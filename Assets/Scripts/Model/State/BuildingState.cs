using UnityEngine;
using UnityEngine.Rendering;

public class BuildingState
{
    public Building Type { get; private set; }
    public float WorkProgress { get; private set; } = 0f;

    public BuildingState(Building type)
    {
        Type = type;
    }

    public void SetWorkProgress(float progress) => WorkProgress = progress;
    public void AdvanceWorkProgress(float amount) => WorkProgress += amount;
}
