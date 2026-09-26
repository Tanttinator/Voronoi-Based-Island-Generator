using UnityEngine;

[CreateAssetMenu(fileName = "Feature", menuName = "Configs/Feature")]
public class Feature : ScriptableObject
{
    public ResourceStack[] GatheringOutput;
    public ResourceStack ClearingOutput;

    public GameObject Prefab;
}
