using UnityEngine;

public class EnemyAi_Tests : EnemyAi
{
    private EnemyTemplate _parentTemplate;

    public override void Update()
    {
        howFar += Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, points[_pointIndex].position, enemyTemplate.speed * Time.deltaTime);
        if (transform.position == points[_pointIndex].position)
        {
            if (_pointIndex != points.Length - 1)
                _pointIndex++;
            else
                _pointIndex = 0;
        }
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _gameManager.money += enemyTemplate.reward;
            if (enemyTemplate.child != null)
                PopulateInfo(enemyTemplate.child, -health);
            else
                PopulateInfo(_parentTemplate, 0);
        }
    }

    public override void PopulateInfo(EnemyTemplate _enemyTemplate, float damage)
    {
        if (_parentTemplate == null)
            _parentTemplate = _enemyTemplate;
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
        TakeDamage(damage);
    }
}
