using StudioIda.Math;
using UnityEngine;

public class MapState
{
    public TileState[] Tiles { get; private set; }
    public Rectangle Area { get; private set; }

    int[][] adjacencies;

    public void SetTiles(TileState[] tiles) => Tiles = tiles;
    public void SetAdjacencies(int[][] adjacencies) => this.adjacencies = adjacencies;
    public int[] GetAdjacencies(int tileId) => adjacencies[tileId];
    public void SetArea(Rectangle area) => Area = area;
}
