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

    [SerializeField] private CameraController _cameraController;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        healthBar.maxValue = health;
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
}
