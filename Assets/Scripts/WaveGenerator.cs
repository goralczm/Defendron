using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    public float enemyIntervals;
    public GameObject enemyPrefab;

    private float timer;

    private void Start()
    {
        timer = enemyIntervals;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            timer = enemyIntervals;
        }
        else
            timer -= Time.deltaTime;
    }
}
