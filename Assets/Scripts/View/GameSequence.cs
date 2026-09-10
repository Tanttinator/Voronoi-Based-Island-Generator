using System.Collections;
using StudioIda.Math;
using UnityEngine;

public abstract class GameSequence
{
    public abstract IEnumerator Display(GameView view, GameConfig config);
}

public class PlaceBuildingSequence : GameSequence
{
    int tileId;

    public PlaceBuildingSequence(int tileId)
    {
        this.tileId = tileId;
    }

    public override IEnumerator Display(GameView view, GameConfig config)
    {
        TileView tile = view.Terrain.Tiles[tileId];
        Point center = tile.Shape.Center;
        GameObject.Instantiate(config.Building.Prefab, new Vector3(center.x, 0f, center.y), Quaternion.identity);
        yield return null;
    }
}

public class ChangeWoodCountSequence : GameSequence
{
    int newCount;

    public ChangeWoodCountSequence(int newCount)
    {
        this.newCount = newCount;
    }

    public override IEnumerator Display(GameView view, GameConfig config)
    {
        view.HUD.SetWoodCount(newCount);
        yield return null;
    }
}
