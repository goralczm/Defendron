using System.Globalization;
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
    public Button targetingButtonLeft;
    public Button targetingButtonRight;
    public TextMeshProUGUI currentTargetingOption;

    public void PopulateInfo(TowerAi tower)
    {
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        targetingButtonLeft.onClick.RemoveAllListeners();
        targetingButtonRight.onClick.RemoveAllListeners();
        currentTargetingOption.text = tower.howToSelectTarget.ToString();
        upgradeButton.interactable = true;

        sellCost.text = tower.ReturnSellCost() + "$";
        sellButton.onClick.AddListener(delegate { GameManager.instance.ReleaseCell(tower); tower.SellTower(); gameObject.SetActive(false); });
        targetingButtonLeft.onClick.AddListener(delegate { tower.ChangeEnemyTargeting(false); currentTargetingOption.text = tower.howToSelectTarget.ToString(); });
        targetingButtonRight.onClick.AddListener(delegate { tower.ChangeEnemyTargeting(true); currentTargetingOption.text = tower.howToSelectTarget.ToString(); });
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
            upgradeButton.onClick.AddListener(delegate { tower.UpgradeTower(true); tower.ShowRangeIndicator(); PopulateInfo(tower); });

            TowerStage currentStage = tower.ReturnCurrentUpgrade();
            TowerStage nextStage = tower.ReturnNextUpgrade();

            upgradesDiffText.text = ReturnFormattedString("Health", currentStage.health, nextStage.health) +
                                    ReturnFormattedString("Damage", currentStage.damage, nextStage.damage) +
                                    ReturnFormattedString("Range", currentStage.range, nextStage.range) +
                                    ReturnFormattedString("Rate of Fire", currentStage.rateOfFire, nextStage.rateOfFire);
        }
    }

    private string ReturnFormattedString(string statName, float firstVal, float secondVal)
    {
        return statName + "\n" + firstVal.ToString(CultureInfo.InvariantCulture) + " > " + secondVal.ToString(CultureInfo.InvariantCulture) + "\n";
    }
}
