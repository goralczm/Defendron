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
}
