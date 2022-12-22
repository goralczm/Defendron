using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveStage
{
    public int enemyCount;
    public float enemyIntervals;
    public GameObject enemyPrefab;
}

public class WaveGenerator : MonoBehaviour
{
    public WaveStage[] waveStages;
    public int currWave;

    private int enemyCount;

    private float timer;

    private void Start()
    {
        timer = waveStages[currWave].enemyIntervals;
        enemyCount = waveStages[currWave].enemyCount;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            if (enemyCount > 0)
            {
                Instantiate(waveStages[currWave].enemyPrefab, transform.position, Quaternion.identity);
                timer = waveStages[currWave].enemyIntervals;
                enemyCount--;
            }
            else
            {
                if (currWave < waveStages.Length - 1)
                    currWave++;
                enemyCount = waveStages[currWave].enemyCount;
            }
        }
        else
            timer -= Time.deltaTime;
    }
}
