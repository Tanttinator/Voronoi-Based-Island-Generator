using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InteractionMode
{
    public GameState State;
    public GameView View;
    public InputProvider Input;
    public GameConfig Config;
    public Action<InteractionMode> ChangeMode;

    public virtual void Start()
    {

    }

    public virtual GameCommand Update()
    {
        return GameCommand.None;
    }

    public virtual void End()
    {

    }
}

public class DefaultInteractionMode : InteractionMode
{
    public override void Start()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected += OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected += OnTaskSelected;
    }

    public override void End()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected -= OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected -= OnTaskSelected;
    }

    void OnBuildingSelected(Building building)
    {
        ChangeMode(new BuildingPlacementMode(building));
    }

    void OnTaskSelected(TaskType task)
    {
        ChangeMode(new TaskCreationMode(task));
    }
}

public class BuildingPlacementMode : InteractionMode
{
    Building selectedBuilding;

    public BuildingPlacementMode(Building building)
    {
        selectedBuilding = building;
    }

    public override void Start()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected += OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected += OnTaskSelected;
    }

    public override GameCommand Update()
    {
        int tileId = View.GetHoveredTile();
        if (tileId != -1)
            View.Terrain.ShowTileOutline(tileId);
        else
            View.Terrain.HideTileOutline();

        if (Input.Clicked)
        {
            return new PlaceConstructionZoneCommand(selectedBuilding, tileId);
        }

        return GameCommand.None;
    }

    public override void End()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected -= OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected -= OnTaskSelected;
    }

    void OnBuildingSelected(Building building)
    {
        ChangeMode(new BuildingPlacementMode(building));
    }

    void OnTaskSelected(TaskType task)
    {
        ChangeMode(new TaskCreationMode(task));
    }
}

public class TaskCreationMode : InteractionMode
{
    TaskType task;

    public TaskCreationMode(TaskType task)
    {
        this.task = task;
    }

    public override void Start()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected += OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected += OnTaskSelected;
    }

    public override GameCommand Update()
    {
        int tileId = View.GetHoveredTile();
        if (tileId != -1)
            View.Terrain.ShowTileOutline(tileId);
        else
            View.Terrain.HideTileOutline();

        if (Input.Clicked)
        {
            if (task == TaskType.GATHER)
                return new PlaceGatheringZoneCommand(tileId);
            else if (task == TaskType.CLEAR)
                return new PlaceClearingZoneCommand(tileId);

            return GameCommand.None;
        }

        return GameCommand.None;
    }

    public override void End()
    {
        View.HUD.ConstructionMenu.OnBuildingSelected -= OnBuildingSelected;
        View.HUD.TaskMenu.OnTaskSelected -= OnTaskSelected;
    }

    void OnBuildingSelected(Building building)
    {
        ChangeMode(new BuildingPlacementMode(building));
    }

    void OnTaskSelected(TaskType task)
    {
        ChangeMode(new TaskCreationMode(task));
    }
}
