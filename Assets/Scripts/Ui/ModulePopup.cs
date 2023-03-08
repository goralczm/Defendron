using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePopup : MonoBehaviour
{
    public PopupPanel popupPanel;
    public Transform availableModulesPanel;
    public GameObject moduleSlotPrefab;

    public List<ModuleButton> buttonList;
    private TowerManager _towerManager;

    public void Setup()
    {
        if (_towerManager == null)
            _towerManager = GameManager.instance.GetComponent<TowerManager>();

        List<Module> upgradeList = _towerManager.availableModules;
        InsertionSort(upgradeList);

        if (availableModulesPanel.childCount == 0)
        {
            foreach (Module module in upgradeList)
            {
                ModuleButton moduleButton = Instantiate(moduleSlotPrefab, availableModulesPanel.position, Quaternion.identity, availableModulesPanel).GetComponent<ModuleButton>();
                moduleButton.modulePopup = gameObject;
                moduleButton.popupPanel = popupPanel;
                moduleButton.Setup(module);
                buttonList.Add(moduleButton);
            }
        }

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < upgradeList.Count; i++)
        {
            buttonList[i].gameObject.SetActive(true);
            buttonList[i].Setup(upgradeList[i]);

            bool foundComplication = false;
            foreach (Module equipedModule in popupPanel.selectedTower.upgrades)
            {
                foreach (Module exclusiveModule in equipedModule.exclusiveModules)
                {
                    if (exclusiveModule == upgradeList[i])
                    {
                        foundComplication = true;
                        break;
                    }
                }
            }

            if (foundComplication)
            {
                buttonList[i].DisableButton();
                continue;
            }

            foreach (Module exclusiveModule in upgradeList[i].exclusiveModules)
            {
                foreach (Module equipedModule in popupPanel.selectedTower.upgrades)
                {
                    if (equipedModule == exclusiveModule)
                    {
                        foundComplication = true;
                        break;
                    }
                }
            }

            if (foundComplication)
            {
                buttonList[i].DisableButton();
                continue;
            }

            if (upgradeList[i].compatibleTowers.Count != 0)
            {
                if (!upgradeList[i].compatibleTowers.Contains(popupPanel.selectedTower.towerTemplate))
                {
                    buttonList[i].gameObject.SetActive(false);
                    continue;
                }
            }

            if (upgradeList[i].uncompatibleTowers.Contains(popupPanel.selectedTower.towerTemplate))
            {
                buttonList[i].gameObject.SetActive(false);
                continue;
            }

            buttonList[i].EnableButton();
        }
    }

    private void InsertionSort(List<Module> modules)
    {
        for (int i = 1; i < modules.Count; i++)
        {
            Module currElemenet = modules[i];
            int previousIndex = i - 1;

            while (previousIndex >= 0 && _towerManager.sortingOrder[modules[previousIndex]] > _towerManager.sortingOrder[currElemenet])
            {
                modules[previousIndex + 1] = modules[previousIndex];
                previousIndex--;
            }

            modules[previousIndex + 1] = currElemenet;
        }
    }
}
