using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    public void Setup(TowerTemplate tower)
    {
        Button button = GetComponent<Button>();
        GetComponent<Image>().sprite = tower.towerLevels[0].sprite;
        button.onClick.AddListener(delegate { GameManager.instance.GetComponent<TowerPlacement>().BuildTower(tower); });
        Destroy(this);
    }
}
