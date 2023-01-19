using UnityEngine;

[System.Serializable]
public class TowerStage
{
    public string name;
    public int cost;
    public int health;
    public int damage;
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
        GameObject tower = new GameObject(towerLevels[0].name);
        SpriteRenderer rend = tower.AddComponent<SpriteRenderer>();
        rend.sprite = towerLevels[0].sprite;
        rend.sortingOrder = 5;
        TowerAi towerAiScript = tower.AddComponent<TowerAi>();
        towerAiScript.towerTemplate = this;
        towerAiScript.isBuilding = true;
        tower.AddComponent<BoxCollider2D>().enabled = false;
        tower.tag = "Tower";
        tower.layer = 7;

        return tower;
    }
}
