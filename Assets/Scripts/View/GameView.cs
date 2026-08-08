using StudioIda.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public Terrain Terrain { get; private set; }

    GameObject buildingPrefab;
    GameObject treePrefab;

    public GameView(GameViewResources resources)
    {
        Terrain = resources.Terrain;
        buildingPrefab = resources.BuildingPrefab;
        treePrefab = resources.TreePrefab;

        Terrain.HideTileOutline();
    }

    public void ProcessEvents(GameEvent[] events)
    {
        foreach (GameEvent evt in events)
        {
            if (evt is BuildingPlacedEvent buildingPlaced)
            {
                TileView tile = Terrain.Tiles[buildingPlaced.TileId];
                Point center = tile.Shape.Center;
                GameObject.Instantiate(buildingPrefab, new Vector3(center.x, 0f, center.y), Quaternion.identity);
            }
        }
    }

    public void GenerateTerrain(Map map)
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
