using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Configs/Building")]
public class BuildingConfig : ScriptableObject
{
    [Header("Model")]
    public int WoodCost;

    [Header("View")]
    public GameObject Prefab;
}
