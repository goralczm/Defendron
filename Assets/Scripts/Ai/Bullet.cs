using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public EnemyAi target;
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [HideInInspector] public int damage;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

        transform.localRotation = targetRot;

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        float distanceBtwTarget = Vector2.Distance(transform.position, target.transform.position);
        
        if (distanceBtwTarget <= 0.05f)
        {
            target.TakeDamage(damage);
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
