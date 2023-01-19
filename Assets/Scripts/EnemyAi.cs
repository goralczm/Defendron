using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public EnemyTemplate enemyTemplate;
    public int health;

    private GameManager _gameManager;
    private SpriteRenderer _spriteRenderer;
    private Transform[] _points;
    private int _pointIndex;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _points = Waypoints.points;
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _points[_pointIndex].position, enemyTemplate.speed * Time.deltaTime);
        if (transform.position == _points[_pointIndex].position)
        {
            if (_pointIndex != _points.Length - 1)
                _pointIndex++;
            else
            {
                Destroy(gameObject);
                _gameManager.TakeDamage(enemyTemplate.health);
            }
        }
    }

    public void CreateChild()
    {
        if (enemyTemplate.child != null)
        {
            Instantiate(gameObject, transform.position, Quaternion.identity).GetComponent<EnemyAi>().PopulateInfo(enemyTemplate.child);
        }
    }

    public void PopulateInfo(EnemyTemplate _enemyTemplate)
    {
        enemyTemplate = _enemyTemplate;
        _spriteRenderer.sprite = enemyTemplate.sprite;
        health = enemyTemplate.health;
    }
}
