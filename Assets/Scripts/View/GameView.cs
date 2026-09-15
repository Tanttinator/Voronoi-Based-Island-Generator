using System.Linq;
using StudioIda.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public HUDView HUD { get; private set; }
    public Terrain Terrain { get; private set; }

    GameObject treePrefab;
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

        treePrefab = resources.TreePrefab;
        taskZoneIndicatorPrefab = resources.TaskZoneIndicator;
    }

    public void Initialize(GameState state)
    {
        foreach (Resource resource in HUD.DisplayResources)
            HUD.UpdateResourceCounter(resource, state.GetResource(resource));
        GenerateTerrain(state.Map);
    }

    void GenerateTerrain(MapState map)
    {
        Terrain.GenerateMesh(map.Tiles, map.Area);
        for (int i = 0; i < map.Tiles.Length; i++)
        {
            if (map.Tiles[i].IsForest)
            {
                TileView tile = Terrain.Tiles[i];

                for (int j = 0; j < 20; j++)
                {
                    Point position = tile.GetRandomPointOnTile();

                    GameObject tree = GameObject.Instantiate(treePrefab, Terrain.transform);
                    tree.transform.position = new Vector3(position.x, 0, position.y);
                }
            }
        }
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
        return view;
    }
}
