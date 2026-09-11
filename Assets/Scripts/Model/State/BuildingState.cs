using UnityEngine;
using UnityEngine.Rendering;

public class BuildingState
{
    public float WorkProgress { get; private set; } = 0f;

    public void SetWorkProgress(float progress) => WorkProgress = progress;
    public void AdvanceWorkProgress(float amount) => WorkProgress += amount;
}
