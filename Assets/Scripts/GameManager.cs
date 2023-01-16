using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    public Material defaultSpriteMat;
    public Material pixelOutlineMat;

    public int health;
    public int money;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "Money: " + money.ToString() + "$";
        healthText.text = "Health: " + health.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
