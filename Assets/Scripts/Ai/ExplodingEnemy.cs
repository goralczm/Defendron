using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : EnemyAi
{
    [SerializeField] private GameObject explosionEffect;

    public override void Die()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        List<TowerAi> towersInRange = GameManager.instance.ReturnNearbyTowers(transform.position, 2);
        foreach (TowerAi tower in towersInRange)
        {
            tower.TakeDamage(enemyTemplate.damage);
        }

        base.Die();
    }
}
