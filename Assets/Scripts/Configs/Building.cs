using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Configs/Building")]
public class Building : ScriptableObject
{
    [Header("Model")]
    public int WoodCost;

    [Header("View")]
    public GameObject Prefab;
}
