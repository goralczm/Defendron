using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private WaveStage[] waveStages;

    public bool gameStarted;
    public int enemiesAlive;

    private GameManager _gameManager;
    private int _currWave;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        wavesText.text = "Wave" + "\n" + (_currWave + 1) + "/" + waveStages.Length;
    }

    public void EnemyDie()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
            EndWave();
    }

    public void EndWave()
    {
        _currWave++;
        if (_currWave >= waveStages.Length - 1)
        {
            _gameManager.ReloadLevel();
            return;
        }

        _gameManager.money += waveStages[_currWave].reward;
        Time.timeScale = 1f;
    }

    public void GenerateCurrentWave(bool forceGenerate)
    {
        if (!forceGenerate)
            if (enemiesAlive > 0)
                return;
        enemiesAlive = 0;
        StartCoroutine(GenerateWaveStage(waveStages[_currWave]));
    }

    IEnumerator GenerateWaveStage(WaveStage wave)
    {
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            StartCoroutine(GenerateEnemiesInWave(wave.enemies[i], wave.enemyIntervals));
            yield return new WaitForSeconds(wave.enemies[i].enemyCount * wave.enemyIntervals);
        }
    }

    IEnumerator GenerateEnemiesInWave(EnemyInWave enemyInfo, float intervalsBtwEnemies)
    {
        enemiesAlive += enemyInfo.enemyCount;
        for (int i = 0; i < enemyInfo.enemyCount; i++)
        {
            EnemyAi enemyAi = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyAi>();
            enemyAi.PopulateInfo(enemyInfo.enemyTemplate, 0);
            enemyAi.points = waypoints.points;
            enemyAi._waveGenerator = this;
            yield return new WaitForSeconds(intervalsBtwEnemies);
        }
    }
}
