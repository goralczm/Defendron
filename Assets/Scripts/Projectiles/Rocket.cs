using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Bullet
{
    [SerializeField] private float explosionRange;

    public override void DamageTarget()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].tag == "Enemy")
            {
                if (hit[i] == null)
                    continue;

                EnemyAi enemy = hit[i].gameObject.GetComponent<EnemyAi>();
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}