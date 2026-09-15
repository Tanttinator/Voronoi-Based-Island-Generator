using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Configs/Building")]
public class Building : ScriptableObject
{
    [Header("Model")]
    public ResourceStack[] Cost;
    public float ConstructionDuration;
    public Job Job;

    [Header("View")]
    public GameObject Prefab;
}
