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

        int cellCount = 10;
        Vector2[] cellPoints = new Vector2[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            cellPoints[i] = new Vector2(x, y);
        }

        (DCEL dcel, Dictionary<Vertex, Point> vertexData) = Delaunay.Triangulate(cellPoints.Select(v => new Point(v.x, v.y)).ToArray());

        List<Polygon> cells = new List<Polygon>();
        foreach (Vertex vertex in dcel.Vertices)
        {
            List<Vector2> points = new List<Vector2>();
            foreach (HalfEdge edge in vertex.OutgoingEdges)
            {
                if (edge.Face != null)
                {
                    points.Add(CalculateTriangleCenter(edge.Face));
                }
                else
                {
                    points.Add(CalculateEdgeIntersection(edge));

                    int edgeA = IntersectingEdge(edge);
                    int edgeB = IntersectingEdge(edge.Prev);
                    if (edgeA != edgeB)
                    {
                        Vector2 corner = Vector2.zero;
                        if (edgeA == 0) corner = new Vector2(maxX, maxY);
                        if (edgeA == 1) corner = new Vector2(maxX, minY);
                        if (edgeA == 2) corner = new Vector2(minX, minY);
                        if (edgeA == 3) corner = new Vector2(minX, maxY);
                        points.Add(corner);
                    }

                    points.Add(CalculateEdgeIntersection(edge.Prev));
                }

            }

            cells.Add(new Polygon(points.ToArray()));

            terrain.GenerateMesh(cells.ToArray());
        }

        Vector2 CalculateTriangleCenter(Face triangle)
        {
            Point[] corners = triangle.Vertices.Select(v => vertexData[v]).ToArray();
            float x = corners[0].x + corners[1].x + corners[2].x;
            float y = corners[0].y + corners[1].y + corners[2].y;
            return new Vector2(x, y) / 3f;
        }

        Vector2 CalculateEdgeIntersection(HalfEdge edge)
        {
            Vector2 a = new Vector2(vertexData[edge.Origin].x, vertexData[edge.Origin].y);
            Vector2 b = new Vector2(vertexData[edge.Next.Origin].x, vertexData[edge.Next.Origin].y);

            Vector2 edgeDir = b - a;
            Vector2 perpendicular = new Vector2(edgeDir.y, -edgeDir.x);
            Vector2 origin = CalculateTriangleCenter(edge.Twin.Face);

            float tx = float.PositiveInfinity;
            float ty = float.PositiveInfinity;

            if (perpendicular.x > 0)
                tx = (maxX - origin.x) / perpendicular.x;
            else if (perpendicular.x < 0)
                tx = (minX - origin.x) / perpendicular.x;

            if (perpendicular.y > 0)
                ty = (maxY - origin.y) / perpendicular.y;
            else if (perpendicular.y < 0)
                ty = (minY - origin.y) / perpendicular.y;

            float t = Mathf.Min(tx, ty);
            Vector2 intersection = origin + perpendicular * t;
            return intersection;
        }

        int IntersectingEdge(HalfEdge edge)
        {
            Vector2 a = new Vector2(vertexData[edge.Origin].x, vertexData[edge.Origin].y);
            Vector2 b = new Vector2(vertexData[edge.Next.Origin].x, vertexData[edge.Next.Origin].y);

            Vector2 edgeDir = b - a;
            Vector2 perpendicular = new Vector2(edgeDir.y, -edgeDir.x);
            Vector2 origin = CalculateTriangleCenter(edge.Twin.Face);

            float tx = float.PositiveInfinity;
            float ty = float.PositiveInfinity;

            if (perpendicular.x > 0)
                tx = (maxX - origin.x) / perpendicular.x;
            else if (perpendicular.x < 0)
                tx = (minX - origin.x) / perpendicular.x;

            if (perpendicular.y > 0)
                ty = (maxY - origin.y) / perpendicular.y;
            else if (perpendicular.y < 0)
                ty = (minY - origin.y) / perpendicular.y;

            if (tx < ty)
            {
                if (perpendicular.x < 0) return 3;
                else return 1;
            }
            else
            {
                if (perpendicular.y < 0) return 2;
                else return 0;
            }
        }
    }

    void Start()
    {
        GenerateCells();
    }
}
