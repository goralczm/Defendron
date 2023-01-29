using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [HideInInspector] public EnemyTemplate enemyTemplate;
    [HideInInspector] public Transform[] points;
    [HideInInspector] public int health;

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
        transform.position = Vector2.MoveTowards(transform.position, points[_pointIndex].position, enemyTemplate.speed * Time.deltaTime);
        if (transform.position == points[_pointIndex].position)
        {
            if (_pointIndex != points.Length - 1)
                _pointIndex++;
            else
            {
                _gameManager.TakeDamage(enemyTemplate.health);
                Die();
            }
        }
    }

    public virtual void TakeDamage(int damage)
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

    public virtual void Die()
    {
        Instantiate(_effectsManager.effects[enemyTemplate.onDieEffect], transform.position, Quaternion.identity);
        _waveGenerator.EnemyDie();
        Destroy(gameObject);
    }

    public virtual void PopulateInfo(EnemyTemplate _enemyTemplate, int damage)
    {
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
        TakeDamage(damage);
    }
}
