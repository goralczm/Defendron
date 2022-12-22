using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float range;
    public int damage;

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].tag == "Enemy")
            {
                EnemyAi enemy = hit[i].gameObject.GetComponent<EnemyAi>();
                enemy.health -= damage;
                if (enemy.health <= 0)
                    Destroy(enemy.gameObject);
                Destroy(gameObject);
            }
            else if (hit[i].tag == "Wall")
                Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
