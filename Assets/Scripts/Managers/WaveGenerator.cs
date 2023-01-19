using UnityEngine;
using TMPro;

[System.Serializable]
public class EnemyInWave
{
    public EnemyTemplate enemyTemplate;
    public int enemyCount;
}

[System.Serializable]
public class WaveStage
{
    public float enemyIntervals;
    public EnemyInWave[] enemies;
    public int reward;
}

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wavesText;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private WaveStage[] waveStages;
    [HideInInspector] public bool gameStarted;
    
    private int _currWave;
    private int _currEnemyInWave;
    private int _enemyCount;
    private float _timer;

    private GameManager _gameManager;

    private void Start()
    {
        _timer = waveStages[_currWave].enemyIntervals;
        _enemyCount = waveStages[_currWave].enemies[0].enemyCount;
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (gameStarted)
        {
            if (_timer <= 0)
            {
                if (_enemyCount > 0)
                {
                    EnemyAi enemyAi = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyAi>();
                    enemyAi.PopulateInfo(waveStages[_currWave].enemies[_currEnemyInWave].enemyTemplate);
                    _timer = waveStages[_currWave].enemyIntervals;
                    _enemyCount--;
                }
                else
                {
                    if (_currEnemyInWave < waveStages[_currWave].enemies.Length - 1)
                    {
                        _currEnemyInWave++;
                    }
                    else
                    {
                        if (_currWave < waveStages.Length - 1)
                            _currWave++;
                        else
                            _currEnemyInWave = 0;
                    }
                    _enemyCount = waveStages[_currWave].enemies[_currEnemyInWave].enemyCount;
                    _gameManager.money += waveStages[_currWave].reward;
                }
            }
            else
                _timer -= Time.deltaTime;
        }

        wavesText.text = "Wave " + _currWave + "/" + waveStages.Length;
    }
}
