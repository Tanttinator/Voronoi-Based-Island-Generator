using System.Collections;
using StudioIda.Math;
using UnityEngine;

public abstract class GameSequence
{
    public abstract IEnumerator Display(GameView view, GameConfig config);
}

public class PlaceBuildingSequence : GameSequence
{
    Building building;
    int tileId;

    public PlaceBuildingSequence(Building building, int tileId)
    {
        this.building = building;
        this.tileId = tileId;
    }

    public override IEnumerator Display(GameView view, GameConfig config)
    {
        TileView tile = view.Terrain.Tiles[tileId];
        Point center = tile.Shape.Center;
        GameObject.Instantiate(building.Prefab, new Vector3(center.x, 0f, center.y), Quaternion.identity);
        yield return null;
    }
}

public class ChangeResourceAmountSequence : GameSequence
{
    ResourceStack[] newAmounts;

    public ChangeResourceAmountSequence(ResourceStack[] newAmounts)
    {
        this.newAmounts = newAmounts;
    }

    public override IEnumerator Display(GameView view, GameConfig config)
    {
        foreach (ResourceStack stack in newAmounts)
        {
            if (view.HUD.HasCounterForResource(stack.Resource))
                view.HUD.UpdateResourceCounter(stack.Resource, stack.Amount);
        }
        yield return null;
    }
}
