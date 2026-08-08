using UnityEngine;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine.InputSystem;

public class GameController
{
    Game model;
    GameView view;

    public GameController(GameViewResources viewResources)
    {
        view = new GameView(viewResources);
    }

    public void StartGame()
    {
        float minX = -5;
        float minY = -5;
        float maxX = 5;
        float maxY = 5;
        Rectangle area = new Rectangle(new Point(minX, minY), new Point(maxX, maxY));

        Tile[] tiles = GenerateMap(area);

        model = new Game(tiles, area);
        view.GenerateTerrain(model.Map);
    }

    public void Update()
    {
        Tile tile = view.GetHoveredTile();
        if (tile != null)
            view.Terrain.ShowTileOutline(tile);
        else
            view.Terrain.HideTileOutline();
    }

    Tile[] GenerateMap(Rectangle area)
    {
        Point[] points = PoissonDisc.SamplePoints(area, 0.5f);

        Tile[] tiles = new Tile[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Point position = points[i];
            tiles[i] = new Tile
            {
                Position = position,
                IsWater = Point.DistanceSqr(area.Center, position) > 16
            };
        }

        return tiles;
    }
}
