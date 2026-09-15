using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public Building[] Buildings;

    public float GatheringZoneDuration;
    public ResourceStack[] GatheringZoneOutput;

    public float ClearingZoneDuration;
    public ResourceStack ClearingZoneOutput;

    [Header("View")]
    public Resource[] DisplayResources;

    public Texture2D ConstructionIcon;
    public Color ConstructionColor;
    public Texture2D ClearingIcon;
    public Color ClearingColor;
    public Texture2D GatheringIcon;
    public Color GatheringColor;
}
