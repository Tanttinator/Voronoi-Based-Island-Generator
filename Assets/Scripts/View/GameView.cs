using UnityEngine;
using UnityEngine.InputSystem;

public class GameView
{
    public Terrain Terrain { get; private set; }

    public GameView(GameViewResources resources)
    {
        Terrain = resources.Terrain;

        Terrain.HideTileOutline();
    }

    public void GenerateTerrain(Map map)
    {
        Terrain.GenerateMesh(map.Tiles, map.Area);
    }

    public Tile GetHoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Tile tile = Terrain.GetCollidedTile((MeshCollider)hit.collider);
            return tile;
        }

        return null;
    }
}
