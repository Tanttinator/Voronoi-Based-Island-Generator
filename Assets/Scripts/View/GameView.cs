using System.Collections.Generic;
using System.Linq;
using StudioIda.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public HUDView HUD { get; private set; }
    public TerrainView Terrain { get; private set; }

    Dictionary<int, TaskZoneIndicatorView> taskZoneIndicators = new Dictionary<int, TaskZoneIndicatorView>();

    GameObject taskZoneIndicatorPrefab;

    public GameView(GameViewResources resources, GameConfig config)
    {
        HUD = new HUDView(resources.HUD.rootVisualElement, resources.ResourceCountTemplate, resources.ConstructionMenuItemTemplate);
        foreach (Resource resource in config.DisplayResources)
            HUD.CreateResourceCounter(resource);
        foreach (Building building in config.Buildings)
            HUD.ConstructionMenu.AddBuilding(building);

        Terrain = resources.Terrain;
        Terrain.HideTileOutline();

        taskZoneIndicatorPrefab = resources.TaskZoneIndicator;
    }

    public void Initialize(GameState state)
    {
        foreach (Resource resource in HUD.DisplayResources)
            HUD.UpdateResourceCounter(resource, state.GetResource(resource));
        Terrain.GenerateTerrain(state.Map);
    }

    public int GetHoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, LayerMask.GetMask("Tile")))
        {
            return Terrain.GetCollidedTile((MeshCollider)hit.collider);
        }

        return -1;
    }

    public TaskZoneIndicatorView CreateTaskZoneIndicator(int tileId, Texture2D icon, Color color)
    {
        TileView tile = Terrain.Tiles[tileId];
        Vector3 position = new Vector3(tile.Shape.Center.x, 0f, tile.Shape.Center.y);
        Vector3[] border = tile.Shape.Corners.Select(p => new Vector3(p.x, 0.01f, p.y)).ToArray();

        TaskZoneIndicatorView view = new TaskZoneIndicatorView(taskZoneIndicatorPrefab, position, border, icon, color);
        taskZoneIndicators[tileId] = view;
        return view;
    }

    public void DestroyTaskZoneIndicator(int tileId)
    {
        TaskZoneIndicatorView view = taskZoneIndicators[tileId];
        view.Destroy();
        taskZoneIndicators.Remove(tileId);
    }
}
