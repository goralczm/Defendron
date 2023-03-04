using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Modules/New Modules")]
public class Module : ScriptableObject
{
    public new string name;
    [TextArea] public string description;
    public TowerType towerTypes;
    public Sprite icon;
    public int tier;

    public int health;
    public float damage;
    public float range;
    public float rateOfFire;
    public int shootMultiplier;
    public string script;
    public GameObject projectile;

    public List<Module> exclusiveModules;
}
