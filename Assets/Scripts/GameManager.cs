using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int money;
    public int health;

    public TextMeshProUGUI moneyText;

    private void Update()
    {
        moneyText.text = "Money: " + money.ToString() + "$";
    }
}
