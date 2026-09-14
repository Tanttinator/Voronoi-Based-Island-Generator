using StudioIda.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public HUDView HUD { get; private set; }
    public Terrain Terrain { get; private set; }

    GameObject treePrefab;

    public GameView(GameViewResources resources, GameConfig config)
    {
        HUD = new HUDView(resources.HUD.rootVisualElement, resources.ResourceCountTemplate);
        foreach (Resource resource in config.DisplayResources)
            HUD.CreateResourceCounter(resource);

        Terrain = resources.Terrain;
        Terrain.HideTileOutline();

        treePrefab = resources.TreePrefab;
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
}
