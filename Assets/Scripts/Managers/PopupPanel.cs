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
    public TextMeshProUGUI upgradesDiffText;

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
            upgradesDiffText.text = "";
        }
        else
        {
            upgradeCost.text = "Cost: " + tower.ReturnUpgradeCost() + "$";
            upgradeButton.onClick.AddListener(delegate { tower.UpgradeTower(true); PopulateInfo(tower); });

            TowerStage currentStage = tower.ReturnCurrentUpgrade();
            TowerStage nextStage = tower.ReturnNextUpgrade();

            upgradesDiffText.text = "Health" + "\n" + currentStage.health + " > " + nextStage.health + "\n" +
                                    "Damage" + "\n" + currentStage.damage + " > " + nextStage.damage + "\n" +
                                    "Range" + "\n" + currentStage.range + " > " + nextStage.range + "\n" +
                                    "Rate of Fire" + "\n" + currentStage.rateOfFire + " > " + nextStage.rateOfFire;
        }
    }
}
