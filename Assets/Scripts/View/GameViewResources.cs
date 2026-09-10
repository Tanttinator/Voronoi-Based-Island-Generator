using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class GameViewResources
{
    [Header("References")]
    public UIDocument HUD;
    public Terrain Terrain;

    [Header("Prefabs")]
    public GameObject BuildingPrefab;
    public GameObject TreePrefab;
}
