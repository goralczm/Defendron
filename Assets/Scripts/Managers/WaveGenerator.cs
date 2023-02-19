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
    public bool completed;
}

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wavesText;
    [SerializeField] private TextMeshProUGUI bossText;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private WaveStage[] waveStages;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    private CameraController _camera;
    private int _currWave;
    public List<Transform> enemiesAlive;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _camera = Camera.main.GetComponent<CameraController>();
        enemiesAlive = new List<Transform>();
    }

    private void Update()
    {
        if (wavesText != null)
            wavesText.text = "Wave" + "\n" + (_currWave + 1) + "/" + waveStages.Length;

        if (waveStages[_currWave].completed && enemiesAlive.Count == 0)
            EndWave();
    }

    public void EndWave()
    {
        if (_currWave == waveStages.Length - 1)
        {
            //_gameManager.ReloadLevel();
            return;
        }
        _gameManager.money += waveStages[_currWave].reward;
        _audioManager.Play("sell");
        Time.timeScale = 1f;
        _currWave++;
    }

    public bool CanStartWave()
    {
        if (enemiesAlive.Count != 0)
            return false;

        return true;
    }

    public void GenerateCurrentWave()
    {
        StartCoroutine(GenerateWaveStage(waveStages[_currWave]));
    }

    IEnumerator GenerateWaveStage(WaveStage wave)
    {
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            StartCoroutine(GenerateEnemiesInWave(wave.enemies[i], wave.enemyIntervals));
            yield return new WaitForSeconds(wave.enemies[i].enemyCount * wave.enemyIntervals);
        }
        wave.completed = true;
    }

    IEnumerator GenerateEnemiesInWave(EnemyInWave enemyInfo, float intervalsBtwEnemies)
    {
        for (int i = 0; i < enemyInfo.enemyCount; i++)
        {
            EnemyAi enemyAi = Instantiate(enemyInfo.enemyTemplate.enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyAi>();
            enemyAi.PopulateInfo(enemyInfo.enemyTemplate, 0);
            enemyAi.points = waypoints.points;
            enemyAi._waveGenerator = this;
            enemiesAlive.Add(enemyAi.transform);

            if (enemyInfo.enemyTemplate.isBoss)
            {
                _camera.FocusOnTarget(enemyAi.transform);
                Time.timeScale = 0.25f;
                bossText.text = enemyInfo.enemyTemplate.name;
                bossText.GetComponent<Animator>().Play("boss_intro");
                yield return new WaitForSecondsRealtime(2f);
                _camera.DefocusTarget();
                yield return new WaitForSecondsRealtime(2f);
                Time.timeScale = 1f;
            }
            yield return new WaitForSeconds(intervalsBtwEnemies);
        }
    }
}
