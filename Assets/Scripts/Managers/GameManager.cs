using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public PopupPanel popupPanel;
    public GameObject rangeIndicator;
    public WaveGenerator[] waveGenerators;
    public Material defaultSpriteMat;
    public Material pixelOutlineMat;

    public int health;
    public int money;

    public List<TowerCell> towerCells;

    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        healthBar.maxValue = health;
        towerCells = new List<TowerCell>();
    }

    private void Update()
    {
        moneyText.text = money.ToString() + "$";
        healthBar.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        _cameraController.TriggerShake();
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public TowerAi CheckCellState(Vector2 cellPos)
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
        foreach (WaveGenerator waveGen in waveGenerators)
        {
            waveGen.gameStarted = true;
        }
        Time.timeScale = 1f;
    }

    public void SpeedButton()
    {
        Time.timeScale = 2.5f;
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReleaseCell(TowerAi tower)
    {
        for (int i = 0; i < towerCells.Count; i++)
        {
            if (towerCells[i].tower == tower)
            {
                towerCells.RemoveAt(i);
            }
        }
    }
}
