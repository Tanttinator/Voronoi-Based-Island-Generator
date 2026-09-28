using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("Model")]
    public Building[] Buildings;

    public WorldGenerationSettings WorldGenerationSettings;

    public float GatheringZoneDuration;
    public float ClearingZoneDuration;

    [Header("View")]
    public Resource[] DisplayResources;

    public Texture2D ConstructionIcon;
    public Color ConstructionColor;
    public Texture2D ClearingIcon;
    public Color ClearingColor;
    public Texture2D GatheringIcon;
    public Color GatheringColor;
}
