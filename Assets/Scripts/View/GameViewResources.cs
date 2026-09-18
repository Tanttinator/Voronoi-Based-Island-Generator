using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class GameViewResources
{
    [Header("References")]
    public UIDocument HUD;
    public TerrainView Terrain;

    [Header("Prefabs")]
    public GameObject TaskZoneIndicator;

    [Header("UI Templates")]
    public VisualTreeAsset ResourceCountTemplate;
    public VisualTreeAsset ConstructionMenuItemTemplate;
}
