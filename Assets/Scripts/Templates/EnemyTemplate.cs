using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Basic Enemy")]
public class EnemyTemplate : ScriptableObject
{
    public new string name;
    public int health;
    public int reward;
    public float speed;
    public Sprite sprite;
    public EnemyTemplate child;
}
