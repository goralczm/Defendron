using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAi : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    [HideInInspector] public bool isBuilding;
    private SpriteRenderer _rangeIndicator;
    private SpriteRenderer _spriteRend;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private Transform _target;
    private float _timer;
    private int _currTowerLevel;

    private void Start()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_rangeIndicator != null)
            _rangeIndicator.transform.position = transform.position;

        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        if (_target == null)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, towerTemplate.towerLevels[_currTowerLevel].range, 1 << 8);
            if (hit != null && _target != hit)
                _target = hit.transform;
        }
        else
        {
            float distanceBtwTarget = Vector2.Distance(transform.position, _target.position);
            if (distanceBtwTarget > towerTemplate.towerLevels[_currTowerLevel].range)
                _target = null;
            else
            {
                if (_timer <= 0)
                {
                    Vector3 dir = _target.position - transform.position;
                    float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

                    Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, targetRot).GetComponent<Bullet>().target = _target.GetComponent<EnemyAi>();
                    _audioManager.Play("bullet");
                    _timer = towerTemplate.towerLevels[_currTowerLevel].rateOfFire;
                }
                else
                {
                    _timer -= Time.deltaTime;
                }
            }
        }
    }

    public bool UpgradeTower()
    {
        if (_currTowerLevel < towerTemplate.towerLevels.Length - 1)
        {
            if (towerTemplate.towerLevels[_currTowerLevel].cost > _gameManager.money)
                return false;
            _currTowerLevel++;
            GetComponent<SpriteRenderer>().sprite = towerTemplate.towerLevels[_currTowerLevel].sprite;
            name = towerTemplate.towerLevels[_currTowerLevel].name;
            _gameManager.money -= towerTemplate.towerLevels[_currTowerLevel].cost;
            ShowRangeIndicator();
            _audioManager.Play("upgrade");
            return true;
        }
        else
            return false;
    }

    public void DestroyTower()
    {
        _gameManager.money += towerTemplate.towerLevels[_currTowerLevel].cost / 2;
        _audioManager.Play("sell");
        HideRangeIndicator();
        Destroy(gameObject);
    }

    public void ShowRangeIndicator()
    {
        HideRangeIndicator();
        if (_rangeIndicator == null)
        {
            if (_gameManager == null)
                _gameManager = GameManager.instance;
            _rangeIndicator = Instantiate(_gameManager.rangeIndicator, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
            float range = towerTemplate.towerLevels[_currTowerLevel].range;
            _rangeIndicator.transform.localScale = new Vector3(range, range, range);
        }
    }

    public void HideRangeIndicator()
    {
        if (_rangeIndicator != null)
        {
            Destroy(_rangeIndicator.gameObject);
            _rangeIndicator = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerTemplate.towerLevels[_currTowerLevel].range);
    }
}
