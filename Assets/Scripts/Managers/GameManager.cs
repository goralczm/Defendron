using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class TowerCell
{
    public Vector2 cellPos;
    public TowerAi tower;
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

    public TowerAi ReturnTowerOnCell(Vector2 pos)
    {
        foreach (TowerCell cell in towerCells)
        {
            if (cell.cellPos == pos)
                return cell.tower;
        }

        return null;
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

    public List<TowerAi> ReturnNearbyTowers(Vector2 pos, int distance)
    {
        List<TowerAi> foundTowers = new List<TowerAi>();

        Vector2[] corners = new Vector2[4];
        corners[0] = new Vector2(pos.x - distance, pos.y + distance);
        corners[1] = new Vector2(pos.x - distance, pos.y - distance);
        corners[2] = new Vector2(pos.x + distance, pos.y + distance);
        corners[3] = new Vector2(pos.x + distance, pos.y - distance);

        for (float w = pos.x - distance; w <= pos.x + distance; w++)
        {
            for (float h = pos.y - distance; h <= pos.y + distance; h++)
            {
                Vector2 cellPos = new Vector2(w, h);
                if (corners.Contains(cellPos))
                    continue;

                //Instantiate(GetComponent<EffectsManager>().effects["explosion"], cellPos, Quaternion.identity);

                Vector2 roundedPos = new Vector2((int)w, (int)h);

                TowerAi towerInCell = ReturnTowerOnCell(roundedPos);
                if (towerInCell != null)
                    foundTowers.Add(towerInCell);
            }
        }

        return foundTowers;
    }
}
