using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private EnemyAi enemy;
    private SpriteRenderer rend;
    public float timer;

    void Start()
    {
        timer = 5;
        enemy = GetComponent<EnemyAi>();
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine(FireDamage());
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            rend.color = Color.white;
            Destroy(this);
        }
    }

    IEnumerator FireDamage()
    {
        rend.color = Color.red;
        enemy.TakeDamage(enemy.enemyTemplate.health / 70);
        yield return new WaitForSeconds(.1f);
        rend.color = Color.white;
        yield return new WaitForSeconds(.1f);
        StartCoroutine(FireDamage());
    }
}
