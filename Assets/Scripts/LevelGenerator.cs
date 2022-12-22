using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;

    public Transform nodesParent;
    public Transform groundParent;

    public ColorToPrefab[] colorMappings;

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(y, x);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
            return;

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.Equals(pixelColor))
            {
                Vector2 targetPos = new Vector2(transform.position.x + x, transform.position.y + y);
                if (colorMapping.isWaypoint)
                    Instantiate(colorMapping.prefab, targetPos, Quaternion.identity, groundParent);
                else
                    Instantiate(colorMapping.prefab, targetPos, Quaternion.identity, nodesParent);
            }
        }
    }
}
