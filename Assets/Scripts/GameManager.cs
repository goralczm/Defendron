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
    public WaveGenerator waveGenerator;
    public Material defaultSpriteMat;
    public Material pixelOutlineMat;

    public int health;
    public int money;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = money.ToString() + "$";
        healthText.text = health.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayButton()
    {
        waveGenerator.gameStarted = true;
        Time.timeScale = 1f;
    }

    public void SpeedButton()
    {
        Time.timeScale = 2.5f;
    }
}
