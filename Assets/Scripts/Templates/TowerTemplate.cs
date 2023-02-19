using UnityEngine;

[System.Serializable]
public class TowerStage
{
    public string name;
    public int cost;
    public int health;
    public float damage;
    public float range;
    public float rateOfFire;
    public Sprite sprite;
    public GameObject bullet;
}

[CreateAssetMenu(menuName = "Tower/Basic Tower")]
public class TowerTemplate : ScriptableObject
{
    public string towerScript;
    public TowerStage[] towerLevels;
}
