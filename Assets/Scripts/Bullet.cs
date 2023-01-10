using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public EnemyAi target;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        float distanceBtwTarget = Vector2.Distance(transform.position, target.transform.position);
        
        if (distanceBtwTarget <= 0.05f)
        {
            target.health -= damage;
            if (target.health <= 0)
            {
                GameManager.instance.money += target.reward;
                Destroy(target.gameObject);
            }
            Destroy(gameObject);
        }
        /*Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].tag == "Enemy")
            {
                EnemyAi enemy = hit[i].gameObject.GetComponent<EnemyAi>();
                enemy.health -= damage;
                if (enemy.health <= 0)
                {
                    GameManager.instance.money += enemy.reward;
                    Destroy(enemy.gameObject);
                }
                Destroy(gameObject);
            }
            else if (hit[i].tag == "Wall")
                Destroy(gameObject);
        }*/
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
