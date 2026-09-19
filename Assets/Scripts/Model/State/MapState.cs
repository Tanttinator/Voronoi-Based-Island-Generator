using System.Linq;
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
    public int GetEdgeToTile(int fromId, int toId)
    {
        int[] adjacencies = this.adjacencies[fromId];
        for (int i = 0; i < adjacencies.Length; i++)
        {
            if (adjacencies[i] == toId) return i;
        }

        return -1;
    }
    public void SetArea(Rectangle area) => Area = area;
}
