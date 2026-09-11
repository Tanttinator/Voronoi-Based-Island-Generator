using UnityEngine;

[CreateAssetMenu(fileName = "Game Config", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    public int WoodHarvestAmount;
    public Building Building;
}
