using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public Transform waypoint;
    public float distance;

    public Point (Transform newWaypoint, float newDistance)
    {
        waypoint = newWaypoint;
        distance = newDistance;
    }
}

public class Waypoints : MonoBehaviour
{
    public List<Point> pointsByDistance;
    public Transform[] points;

    private void Awake()
    {
        pointsByDistance = new List<Point>();
        points = new Transform[transform.childCount - 1];
        for (int i = 1; i < transform.childCount; i++)
        {
            points[i - 1] = transform.GetChild(i);

            float distance = 0;
            if (i > 1)
            {
                distance = Vector2.Distance(points[i - 2].position, points[i - 1].position);
                distance += pointsByDistance[pointsByDistance.Count - 1].distance;
            }
            else
                distance = Vector2.Distance(transform.GetChild(0).position, points[i - 1].position);

            Point newPoint = new Point(points[i - 1], distance);

            pointsByDistance.Add(newPoint);
        }
    }
}
