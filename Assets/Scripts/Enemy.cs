using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;

    private Transform[] points;
    private int pointIndex;

    private void Start()
    {
        points = Waypoints.points;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].position, speed * Time.deltaTime);
        if (transform.position == points[pointIndex].position)
        {
            if (pointIndex != points.Length - 1)
                pointIndex++;
            else
                Destroy(gameObject);
        }
    }
}
