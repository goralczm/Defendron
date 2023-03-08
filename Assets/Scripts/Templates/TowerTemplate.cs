using UnityEngine;

[System.Serializable]
public class TowerLevel
{
    public float damage;
    public float range;
    public float rateOfFire;
    public int shootsMultiplier;
    public int moduleSlots;

    public int cost;
    public Sprite sprite;
    public GameObject bullet;
}

[CreateAssetMenu(menuName = "Tower/New Tower")]
public class TowerTemplate : ScriptableObject
{
    public new string name;
    public TowerLevel[] levels;
    public GameObject specialPrefab;
    public string script;
    public string onDestroyEffect;
}
