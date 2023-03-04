using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class TowerCell
{
    public Vector2 cellPos;
    public Tower tower;
}

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject menu;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public PopupPanel popupPanel;
    public GameObject rangeIndicator;
    public WaveGenerator[] waveGenerators;
    public TowerManager towerManager;
    public Material defaultSpriteMat;
    public Material pixelOutlineMat;

    public int health;
    public int money;

    public List<TowerCell> towerCells;

    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        healthBar.maxValue = health;
        towerCells = new List<TowerCell>();
    }

    private void Update()
    {
        moneyText.text = money.ToString();
        healthBar.value = health;
        healthText.text = health.ToString() + "/" + 100;
    }

    public void TakeDamage(float damage)
    {
        health -= Mathf.RoundToInt(damage);
        _cameraController.TriggerShake();
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int RoundDamage(float damage)
    {
        return (int)((damage / 10) + 1);
    }

    public Tower CheckCellState(Vector2 cellPos)
    {
        for (int i = 0; i < towerCells.Count; i++)
        {
            if (towerCells[i].cellPos == cellPos)
                return towerCells[i].tower;
        }

        return null;
    }

    public void PlayButton()
    {
        Time.timeScale = 1f;
        for (int i = 0; i < waveGenerators.Length; i++)
        {
            if (!waveGenerators[i].CanStartWave())
                return;
        }

        for (int i = 0; i < waveGenerators.Length; i++)
        {
            waveGenerators[i].GenerateCurrentWave();
        }

        foreach (TowerCell occupiedCell in towerCells)
        {
            occupiedCell.tower.paused = false;
        }
    }

    public void PauseTowers()
    {
        foreach (TowerCell occupiedCell in towerCells)
        {
            occupiedCell.tower.paused = true;
        }
    }

    public void SpeedButton()
    {
        Time.timeScale = 2.5f;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Tower ReturnTowerOnCell(Vector2 pos)
    {
        foreach (TowerCell cell in towerCells)
        {
            if (cell.cellPos == pos)
                return cell.tower;
        }

        return null;
    }

    public void ReleaseCell(Tower tower)
    {
        for (int i = 0; i < towerCells.Count; i++)
        {
            if (towerCells[i].tower == tower)
            {
                towerCells.RemoveAt(i);
            }
        }
    }

    public void PauseGame()
    {
        if (Time.timeScale == 0)
        {
            ResumeGame();
            return;
        }
        menu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
