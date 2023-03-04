using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(TowerTemplate newTower)
    {
        Button button = GetComponent<Button>();
        GetComponent<Image>().sprite = newTower.levels[0].sprite;
        button.onClick.AddListener(delegate { GameManager.instance.GetComponent<TowerManager>().BuildTower(newTower); });
        costText.text = newTower.levels[0].cost.ToString() + "$";
        Destroy(this);
    }
}
