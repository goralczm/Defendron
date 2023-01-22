using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public EnemyTemplate enemyTemplate;
    public Transform[] points;
    public int health;

    private GameManager _gameManager;
    private SpriteRenderer _spriteRenderer;
    private int _pointIndex;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[_pointIndex].position, enemyTemplate.speed * Time.deltaTime);
        if (transform.position == points[_pointIndex].position)
        {
            if (_pointIndex != points.Length - 1)
                _pointIndex++;
            else
            {
                _gameManager.TakeDamage(enemyTemplate.health);
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            _gameManager.money += enemyTemplate.reward;
            if (enemyTemplate.child != null)
                PopulateInfo(enemyTemplate.child);
            else
                Destroy(gameObject);
        }
    }

    public void PopulateInfo(EnemyTemplate _enemyTemplate)
    {
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
    }
}
