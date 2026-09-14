using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public Resource Wood;
    public Building[] Buildings;

    [Header("View")]
    public Resource[] DisplayResources;
}
