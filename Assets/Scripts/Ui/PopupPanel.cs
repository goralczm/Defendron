using System.Globalization;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PopupPanel : MonoBehaviour
{
    public TextMeshProUGUI towerName;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellCost;
    public TextMeshProUGUI upgradesDiffText;
    public Button targetingButtonLeft;
    public Button targetingButtonRight;
    public TextMeshProUGUI currentTargetingOption;
    public ModuleSlot lastModuleButton;
    public List<ModuleSlot> moduleSlots;

    public GameObject moduleSlotPrefab;
    public ModulePopup modulesPopup;
    private TowerManager _towerManager;
    public Tower selectedTower;

    public void PopulateInfo(Tower tower)
    {
        if (_towerManager == null)
            _towerManager = GameManager.instance.GetComponent<TowerManager>();

        selectedTower = tower;
        modulesPopup.gameObject.SetActive(false);

        currentTargetingOption.text = tower.howToSelectTarget.ToString();
        sellCost.text = tower.ReturnSellCost().ToString();
        towerName.text = tower.transform.name;

        RefreshModuleSlots();

        UpdateAvailableModules();

        int upgradeCost = tower.ReturnUpgradeCost();
        if (upgradeCost == 0)
        {
            upgradeCostText.text = "MAX UPGRADE";
            upgradeButton.interactable = false;
            upgradesDiffText.text = tower.DisplayStats();
        }
        else
        {
            upgradeCostText.text = "Cost: " + upgradeCost;
            upgradeButton.interactable = true;
            upgradesDiffText.text = tower.DisplayUpgradeStats();
        }
    }

    public void UpgradeButton()
    {
        selectedTower.UpgradeTower();
        PopulateInfo(selectedTower);
    }

    public void SellButton()
    {
        for (int i = selectedTower.upgrades.Count - 1; i >= 0; i--)
        {
            DemountUpgrade(selectedTower.upgrades[i]);
        }
        selectedTower.SellTower();
        gameObject.SetActive(false);
    }

    public void EnemyTargetingButton(bool next)
    {
        selectedTower.ChangeEnemyTargeting(next);
        selectedTower.ResetTarget();
        PopulateInfo(selectedTower);
    }
    
    public void SelectUpgrade(int upgradeIndex)
    {
        if (!selectedTower.MountUpgrade(_towerManager.availableModules[upgradeIndex]))
            return;
        _towerManager.availableModules.RemoveAt(upgradeIndex);
        UpdateAvailableModules();
    }

    public void DemountUpgrade(Module module)
    {
        selectedTower.DemountUpgrade(module);
        _towerManager.availableModules.Add(module);
        UpdateAvailableModules();
        RefreshModuleSlots();
        PopulateInfo(selectedTower);
    }

    public void MountUpgrade(Module module)
    {
        if (lastModuleButton.module != null)
        {
            bool mounted = selectedTower.SwapModules(lastModuleButton.module, module);
            if (!mounted)
                return;
            _towerManager.availableModules.Add(lastModuleButton.module);
            UpdateAvailableModules();
        }
        else
        {
            bool mounted = selectedTower.MountUpgrade(module);
            if (!mounted)
                return;
        }

        _towerManager.availableModules.Remove(module);
        UpdateAvailableModules();
        RefreshModuleSlots();
        PopulateInfo(selectedTower);
    }

    public void UpdateAvailableModules()
    {
        modulesPopup.Setup();
    }

    public void RefreshModuleSlots()
    {
        for (int i = 0; i < moduleSlots.Count; i++)
        {
            moduleSlots[i].Reset();

            if (i >= selectedTower._currTowerLevel.moduleSlots)
            {
                moduleSlots[i].gameObject.SetActive(false);
                continue;
            }

            moduleSlots[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < selectedTower.upgrades.Count; i++)
        {
            moduleSlots[i].Setup(selectedTower.upgrades[i]);
        }
    }
}
