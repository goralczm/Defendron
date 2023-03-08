using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class EnemyInWave
{
    public EnemyTemplate enemyTemplate;
    public int enemyCount;
    public float pauseInterval;
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
    [SerializeField] private GameObject directionArrows;
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private WaveStage[] waveStages;

    private GameManager _gameManager;
    private AudioManager _audioManager;
    private CameraController _camera;
    private int _currWave;
    private bool _isGenerating;
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

        if (!_isGenerating && waveStages[_currWave].completed && enemiesAlive.Count == 0)
            EndWave();
    }

    public void EndWave()
    {
        if (_currWave == waveStages.Length - 1)
        {
            string sceneName = SceneManager.GetActiveScene().name;
            PlayerPrefs.SetInt(sceneName, 1);
            if (sceneName.Length == 11)
                PlayerPrefs.SetInt("act" + sceneName[3], 1);
            TransitionManager.instance.ChangeScene("level_select");
            return;
        }
        _gameManager.money += waveStages[_currWave].reward;
        //_gameManager.PauseTowers();
        _audioManager.Play("sell");
        Time.timeScale = 1f;
        _currWave++;
    }

    public bool CanStartWave()
    {
        if (enemiesAlive.Count != 0 || _isGenerating)
            return false;

        return true;
    }

    public void GenerateCurrentWave()
    {
        StartCoroutine(GenerateWaveStage(waveStages[_currWave]));
        directionArrows.SetActive(false);
    }

    IEnumerator GenerateWaveStage(WaveStage wave)
    {
        _isGenerating = true;
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            StartCoroutine(GenerateEnemiesInWave(wave.enemies[i], wave.enemyIntervals));
            yield return new WaitForSeconds(wave.enemies[i].enemyCount * wave.enemyIntervals + wave.enemies[i].pauseInterval);
        }
        wave.completed = true;
        _isGenerating = false;
    }

    IEnumerator GenerateEnemiesInWave(EnemyInWave enemyInfo, float intervalsBtwEnemies)
    {
        for (int i = 0; i < enemyInfo.enemyCount; i++)
        {
            EnemyAi enemyAi = Instantiate(enemyInfo.enemyTemplate.enemyPrefab, transform.position, Quaternion.identity).GetComponent<EnemyAi>();
            enemyAi.PopulateInfo(enemyInfo.enemyTemplate, 0);
            enemyAi.points = waypoints.pointsByDistance;
            enemyAi._waveGenerator = this;
            enemiesAlive.Add(enemyAi.transform);

            if (enemyInfo.enemyTemplate.isBoss)
            {
                _camera.FocusOnTarget(enemyAi.transform);
                enemyAi.healthBar.gameObject.SetActive(true);
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
