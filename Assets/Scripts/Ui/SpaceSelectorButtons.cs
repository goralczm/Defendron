using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSelectorButtons : MonoBehaviour
{
    [ContextMenu("Evenly Space Items")]
    public void SpaceItems()
    {
        float xPos = 100;
        float yPos = -30;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i % 2 != 0)
                yPos = 0;
            else if (i % 2 == 0 && i % 4 != 0)
                yPos = 30f;
            else if (i % 4 == 0)
                yPos = -30f;

            transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
            xPos += 90;
        }
    }
}
