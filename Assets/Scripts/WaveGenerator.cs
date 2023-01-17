using UnityEngine;
using TMPro;

[System.Serializable]
public class WaveStage
{
    public int enemyCount;
    public float enemyIntervals;
    public GameObject[] enemyPrefab;

    public GameObject ReturnRandomEnemy()
    {
        return enemyPrefab[Random.Range(0, enemyPrefab.Length)];
    }
}

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wavesText;
    [SerializeField] private WaveStage[] waveStages;
    [HideInInspector] public bool gameStarted;
    
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
        if (gameStarted)
        {
            if (_timer <= 0)
            {
                if (_enemyCount > 0)
                {
                    Instantiate(waveStages[_currWave].ReturnRandomEnemy(), transform.position, Quaternion.identity);
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

        wavesText.text = "Wave " + _currWave + "/" + waveStages.Length;
    }
}
