using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Configs/Building")]
public class Building : ScriptableObject
{
    [Header("Model")]
    public int WoodCost;
    public float WorkDuration;

    [Header("View")]
    public GameObject Prefab;
}
