using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject popupPanel;
    private GameManager gameManager;
    private TowerManager towerManager;

    void Start()
    {
        gameManager = GameManager.instance;
        towerManager = gameManager.GetComponent<TowerManager>();
        popupPanel = gameManager.popupPanel.gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TransitionManager.instance.isChangingScene)
                return;

            if (!popupPanel.activeSelf && !towerManager.isBuilding)
                gameManager.PauseGame();

            towerManager.isBuilding = false;
            towerManager.DeselectTarget();
        }
    }
}
