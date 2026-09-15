using UnityEngine;
using StudioIda.Math;
using System.Collections.Generic;
using System.Collections;

public class GameController
{
    GameConfig config;
    GameModel model;
    GameView view;

    InputProvider input;
    InteractionMode currentInteractionMode;

    Queue<GameSequence> sequenceQueue = new Queue<GameSequence>();
    WaitForEndOfFrame wait = new WaitForEndOfFrame();

    public GameController(GameConfig config, GameViewResources viewResources)
    {
        this.config = config;
        model = new GameModel(config);
        view = new GameView(viewResources, config);

        input = new InputProvider();
    }

    public void StartGame()
    {
        float minX = -5; float maxX = 5;
        float minY = -5; float maxY = 5;
        Rectangle area = new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
        model.State.Map.SetArea(area);

        TileState[] tiles = WorldGeneration.GenerateTiles(area);
        model.State.Map.SetTiles(tiles);

        view.Initialize(model.State);

        SetInteractionMode(new DefaultInteractionMode());
    }

    public void Update()
    {
        input.Update();

        GameCommand command = currentInteractionMode.Update();
        GameEvent[] inputEvents = model.ExecuteCommand(command);
        ProcessEvents(inputEvents);

        GameEvent[] simulationEvents = model.Simulate(Time.deltaTime);
        ProcessEvents(simulationEvents);
    }

    void SetInteractionMode(InteractionMode mode)
    {
        mode.State = model.State;
        mode.View = view;
        mode.Input = input;
        mode.Config = config;

        currentInteractionMode?.End();
        currentInteractionMode = mode;
        currentInteractionMode.Start();
    }

    void ProcessEvents(GameEvent[] events)
    {
        foreach (GameEvent evt in events)
        {
            switch (evt)
            {
                case BuildingPlacedEvent buildingPlaced:
                    sequenceQueue.Enqueue(new PlaceBuildingSequence(buildingPlaced.Building, buildingPlaced.TileId));
                    break;
                case ResourceAmountsChangedEvent resourcesChanged:
                    sequenceQueue.Enqueue(new ChangeResourceAmountSequence(resourcesChanged.NewAmounts));
                    break;
                case GatheringZonePlacedEvent gatheringZonePlaced:
                    view.CreateTaskZoneIndicator(gatheringZonePlaced.TileId, config.GatheringIcon, config.GatheringColor);
                    break;
            }
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
}
