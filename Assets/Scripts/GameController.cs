using UnityEngine;
using StudioIda.Graphs;
using System.Collections.Generic;
using System.Linq;
using StudioIda.Math;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject cellMarker;
    [SerializeField] LineRenderer edgeMarker;
    [SerializeField] Terrain terrain;

    void GenerateCells()
    {
        float minX = -5;
        float minY = -5;
        float maxX = 5;
        float maxY = 5;
        Rectangle area = new Rectangle(new Point(minX, minY), new Point(maxX, maxY));

        Point[] points = PoissonDisc.SamplePoints(area, 1f);
        DCEL dcel = Delaunay.Triangulate(points);
        Polygon[] cells = Voronoi.FromDelaunay(dcel, area);

        terrain.GenerateMesh(cells);
    }

    void Start()
    {
        GenerateCells();
    }
}
