using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopupPanel : MonoBehaviour
{
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI sellCost;

    public void PopulateInfo(TowerAi tower)
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        upgradeButton.interactable = true;

        sellCost.text = "Sell cost: " + tower.ReturnSellCost() + "$";
        sellButton.onClick.AddListener(delegate { tower.DestroyTower(); gameObject.SetActive(false); });

        if (tower.ReturnUpgradeCost() == 0)
        {
            upgradeCost.text = "MAX UPGRADE";
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeCost.text = "Upgrade cost: " + tower.ReturnUpgradeCost() + "$";
            upgradeButton.onClick.AddListener(delegate { tower.UpgradeTower(true); PopulateInfo(tower); });
        }
    }
}
