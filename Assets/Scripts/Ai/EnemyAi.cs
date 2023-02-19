using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float currentSpeed;
    public float startSpeed;
    [HideInInspector] public EnemyTemplate enemyTemplate;
    [HideInInspector] public Transform[] points;
    public float health;
    public float howFar;

    [HideInInspector] public bool isLastEnemy;
    [HideInInspector] public WaveGenerator _waveGenerator;
    [HideInInspector] public GameManager _gameManager;
    [HideInInspector] public EffectsManager _effectsManager;
    [HideInInspector] public SpriteRenderer _spriteRenderer;
    [HideInInspector] public int _pointIndex;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameManager.instance;
        _effectsManager = _gameManager.GetComponent<EffectsManager>();
    }

    public virtual void Update()
    {
        howFar = CalculateDistanceFromStart();
        transform.position = Vector2.MoveTowards(transform.position, points[_pointIndex].position, currentSpeed * Time.deltaTime);
        if (transform.position == points[_pointIndex].position)
        {
            if (_pointIndex != points.Length - 1)
                _pointIndex++;
            else
            {
                _gameManager.TakeDamage(health);
                Die();
            }
        }
    }

    private float CalculateDistanceFromStart()
    {
        if (_pointIndex == 0)
            return 0;

        float distance = 0;
        for (int i = 1; i < _pointIndex; i++)
        {
            distance += Vector2.Distance(points[i - 1].position, points[i].position);
        }
        distance += Vector2.Distance(points[_pointIndex - 1].position, transform.position);

        return distance;
    }

    public virtual void TakeDamage(float damage)
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

    public virtual void PopulateInfo(EnemyTemplate _enemyTemplate, float damage)
    {
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
        startSpeed = enemyTemplate.speed;
        currentSpeed = startSpeed;
        TakeDamage(damage);
    }

    public virtual void Die()
    {
        Instantiate(_effectsManager.effects[enemyTemplate.onDieEffect], transform.position, Quaternion.identity);
        _waveGenerator.enemiesAlive.Remove(transform);
        Destroy(gameObject);
    }
}
