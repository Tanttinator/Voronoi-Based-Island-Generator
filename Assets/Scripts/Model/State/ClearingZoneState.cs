using UnityEngine;

public class ClearingZoneState
{
    public float Progress { get; private set; }

    public void SetProgress(float progress) => Progress = progress;
    public void AdvanceProgress(float amount) => Progress += amount;
}
