using UnityEngine;

[System.Serializable]
public class TowerStage
{
    public string name;
    public int health;
    public float range;
    public float rateOfFire;
    public Sprite sprite;
    public GameObject bullet;
}

[CreateAssetMenu(menuName = "Tower/Basic Tower")]
public class TowerTemplate : ScriptableObject
{
    public TowerStage[] towerLevels;

    public GameObject CreateTower()
    {
        GameObject tower = new GameObject();
        SpriteRenderer rend = tower.AddComponent<SpriteRenderer>();
        rend.sprite = towerLevels[0].sprite;
        rend.sortingOrder = 5;
        tower.name = towerLevels[0].name;
        TowerAi towerAiScript = tower.AddComponent<TowerAi>();
        towerAiScript.towerTemplate = this;
        towerAiScript.enabled = false;
        tower.AddComponent<BoxCollider2D>().enabled = false;
        tower.tag = "Tower";
        tower.layer = 6;

        return tower;
    }
}
