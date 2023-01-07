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
    [SerializeField] private WaveStage[] waveStages;
    
    private int _currWave;
    private int _enemyCount;
    private float _timer;

    private void Start()
    {
        _timer = waveStages[_currWave].enemyIntervals;
        _enemyCount = waveStages[_currWave].enemyCount;
    }

    private void Update()
    {
        if (_timer <= 0)
        {
            if (_enemyCount > 0)
            {
                Instantiate(waveStages[_currWave].enemyPrefab, transform.position, Quaternion.identity);
                _timer = waveStages[_currWave].enemyIntervals;
                _enemyCount--;
            }
            else
            {
                if (_currWave < waveStages.Length - 1)
                    _currWave++;
                _enemyCount = waveStages[_currWave].enemyCount;
            }
        }
        else
            _timer -= Time.deltaTime;
    }
}
