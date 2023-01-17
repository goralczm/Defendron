using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopupPanel : MonoBehaviour
{
    public TextMeshProUGUI towerName;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellCost;

    public void PopulateInfo(TowerAi tower)
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        upgradeButton.interactable = true;

        sellCost.text = tower.ReturnSellCost() + "$";
        sellButton.onClick.AddListener(delegate { tower.DestroyTower(); gameObject.SetActive(false); });
        towerName.text = tower.name;

        if (tower.ReturnUpgradeCost() == 0)
        {
            upgradeCost.text = "MAX UPGRADE";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeCost.text = "Cost: " + tower.ReturnUpgradeCost() + "$";
            upgradeButton.onClick.AddListener(delegate { tower.UpgradeTower(true); PopulateInfo(tower); });
        }
    }
}
