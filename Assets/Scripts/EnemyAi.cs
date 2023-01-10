using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public int health;
    public int reward;

    [SerializeField] private float speed;

    private GameManager _gameManager;
    private Transform[] _points;
    private int _pointIndex;

    private void Start()
    {
        _points = Waypoints.points;
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _points[_pointIndex].position, speed * Time.deltaTime);
        if (transform.position == _points[_pointIndex].position)
        {
            if (_pointIndex != _points.Length - 1)
                _pointIndex++;
            else
            {
                Destroy(gameObject);
                _gameManager.TakeDamage(health);
            }
        }
    }
}
