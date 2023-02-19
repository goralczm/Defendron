using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piercing : Bullet
{
    private Vector2 startPos;
    private List<Transform> enemiesHit;

    private void Start()
    {
        startPos = transform.position;
        enemiesHit = new List<Transform>();
    }

    public override void Update()
    {
        float distance = Vector2.Distance(startPos, transform.position);
        if (distance > range)
            DestroyBullet();

        transform.Translate(Vector2.down * speed * Time.deltaTime);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f, 1 << 8);
        foreach (Collider2D hit in hits)
        {
            if (!enemiesHit.Contains(hit.transform))
            {
                hit.GetComponent<EnemyAi>().TakeDamage(damage);
                enemiesHit.Add(hit.transform);
                break;
            }
        }
    }
}
