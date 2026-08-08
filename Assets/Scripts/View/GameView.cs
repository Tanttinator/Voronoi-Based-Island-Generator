using StudioIda.Math;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public Terrain Terrain { get; private set; }

    GameObject buildingPrefab;

    public GameView(GameViewResources resources)
    {
        Terrain = resources.Terrain;
        buildingPrefab = resources.BuildingPrefab;

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
