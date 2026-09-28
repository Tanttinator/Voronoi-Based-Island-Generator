using System;
using StudioIda.RTSCamera;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class GameViewResources
{
    [Header("References")]
    public UIDocument HUD;
    public TerrainView Terrain;
    public RTSCamera CameraRig;

    [Header("Prefabs")]
    public GameObject TaskZoneIndicator;

    [Header("UI Templates")]
    public VisualTreeAsset ResourceCountTemplate;
    public VisualTreeAsset ConstructionMenuItemTemplate;
}
