using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(TowerTemplate tower)
    {
        Button button = GetComponent<Button>();
        GetComponent<Image>().sprite = tower.towerLevels[0].sprite;
        button.onClick.AddListener(delegate { GameManager.instance.GetComponent<TowerManager>().BuildTower(tower); });
        costText.text = tower.towerLevels[0].cost.ToString() + "$";
        Destroy(this);
    }
}
