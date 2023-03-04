using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float currentSpeed;
    public float startSpeed;
    public float health;
    public float howFar;

    [HideInInspector] public EnemyTemplate enemyTemplate;
    [HideInInspector] public List<Point> points;
    [HideInInspector] public WaveGenerator _waveGenerator;
    [HideInInspector] public Transform healthBar;

    private GameManager _gameManager;
    private EffectsManager _effectsManager;
    private SpriteRenderer _spriteRenderer;
    private Point currTargetPoint;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameManager.instance;
        _effectsManager = EffectsManager.instance;
        healthBar = transform.GetChild(0).transform;
    }

    public void Update()
    {
        howFar = CalculateDistanceFromStart();
        currTargetPoint = GetNextPoint();
        transform.position = Vector2.MoveTowards(transform.position, currTargetPoint.waypoint.position, currentSpeed * Time.deltaTime);
        if (transform.position == points[points.Count - 1].waypoint.position)
        {
            _gameManager.TakeDamage(ReturnLeftHealth());
            Die();
        }


        if (enemyTemplate.isBoss)
            healthBar.localScale = new Vector3(health / enemyTemplate.health * 0.8f, healthBar.localScale.y, 1);
    }

    private float CalculateDistanceFromStart()
    {
        if (currTargetPoint == null)
            return 0;

        float distance = points[points.IndexOf(currTargetPoint)].distance - Vector2.Distance(currTargetPoint.waypoint.position, transform.position);

        return distance;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _gameManager.money += enemyTemplate.reward;
            if (enemyTemplate.child != null)
                PopulateInfo(enemyTemplate.child, -health);
            else
                Die();
        }
    }

    public void PopulateInfo(EnemyTemplate _enemyTemplate, float damage)
    {
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
        startSpeed = enemyTemplate.speed;
        currentSpeed = startSpeed;
        TakeDamage(damage);
    }

    public float ReturnLeftHealth()
    {
        float healthLeft = health;

        EnemyTemplate currTemplate = enemyTemplate;
        while (currTemplate.child != null)
        {
            healthLeft += currTemplate.child.health;
            currTemplate = currTemplate.child;
        }

        return healthLeft;
    }

    private Point GetNextPoint()
    {
        foreach (Point point in points)
        {
            if (point.distance > howFar)
                return point;
        }

        return null;
    }

    public void SetNextPoint(Transform nextPoint)
    {
        foreach (Point point in points)
        {
            if (point.waypoint == nextPoint)
            {
                currTargetPoint = point;
                return;
            }
        }
    }

    public void Die()
    {
        if (enemyTemplate == null)
            return;
        _effectsManager.PlayEffect(enemyTemplate.onDieEffect, transform.position);
        _waveGenerator.enemiesAlive.Remove(transform);
        Destroy(gameObject);
    }
}
