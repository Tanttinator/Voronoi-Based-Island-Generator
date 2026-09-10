using UnityEngine;
using StudioIda.Graphs;
using StudioIda.Math;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class GameController
{
    GameConfig config;

    Game model;
    GameView view;

    Queue<GameSequence> sequenceQueue = new Queue<GameSequence>();
    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    public GameController(GameConfig config, GameViewResources viewResources)
    {
        this.config = config;
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

        view.Initialize(model);
    }

    public void Update()
    {
        int tileId = view.GetHoveredTile();
        if (tileId != -1)
            view.Terrain.ShowTileOutline(tileId);
        else
            view.Terrain.HideTileOutline();

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tileId == -1) return;

            Tile tile = model.Map.Tiles[tileId];
            if (tile.IsForest)
                ProcessCommand(new GatherWoodCommand(tileId));
            else
                ProcessCommand(new PlaceBuildingCommand(tileId));
        }
    }

    public IEnumerator PlaySequences()
    {
        while (true)
        {
            if (sequenceQueue.Count > 0)
            {
                GameSequence sequence = sequenceQueue.Dequeue();
                yield return sequence.Display(view, config);
            }
            yield return wait;
        }
    }

    void ProcessCommand(GameCommand command)
    {
        GameEvent[] events = command.Execute(model, config);

        foreach (GameEvent evt in events)
        {
            switch (evt)
            {
                case BuildingPlacedEvent buildingPlaced:
                    sequenceQueue.Enqueue(new PlaceBuildingSequence(buildingPlaced.TileId));
                    break;
                case WoodCountChangedEvent woodChanged:
                    sequenceQueue.Enqueue(new ChangeWoodCountSequence(woodChanged.NewCount));
                    break;
            }
        }
    }

    Tile[] GenerateMap(Rectangle area)
    {
        Point[] points = PoissonDisc.SamplePoints(area, 0.5f);

        Tile[] tiles = new Tile[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Point position = points[i];
            bool isWater = Point.DistanceSqr(area.Center, position) > 16;
            bool isForest = !isWater && Random.value < 0.5f;
            tiles[i] = new Tile
            {
                Position = position,
                IsWater = isWater,
                IsForest = isForest
            };
        }

        return tiles;
    }
}
