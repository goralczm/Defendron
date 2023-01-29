using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAi : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    [SerializeField] private GameObject onDestroyEffect;
    [HideInInspector] public bool isBuilding;
    public int health;

    private SpriteRenderer _rangeIndicator;
    private SpriteRenderer _spriteRend;
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private EffectsManager _effectsManager;
    private Transform _target;

    private float _timer;
    private int _currTowerLevel;

    private void Awake()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _effectsManager = _gameManager.GetComponent<EffectsManager>();
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
            {
                _target = hit.transform;
            }
            else
                return;
        }

        float distanceBtwTarget = Vector2.Distance(transform.position, _target.position);
        if (distanceBtwTarget > towerTemplate.towerLevels[_currTowerLevel].range)
            _target = null;
        else
        {
            if (_timer <= 0)
            {
                Vector3 dir = _target.transform.position - transform.position;
                float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

                Bullet tmpBulletScript = Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, targetRot).GetComponent<Bullet>();
                tmpBulletScript.target = _target.GetComponent<EnemyAi>();
                tmpBulletScript.damage = towerTemplate.towerLevels[_currTowerLevel].damage;
                _audioManager.Play("bullet");
                _timer = towerTemplate.towerLevels[_currTowerLevel].rateOfFire;
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
    }

    public bool UpgradeTower(bool pay)
    {
        if (_currTowerLevel == towerTemplate.towerLevels.Length - 1)
            return false;

        if (towerTemplate.towerLevels[_currTowerLevel].cost > _gameManager.money && pay)
            return false;

        if (pay)
            _gameManager.money -= towerTemplate.towerLevels[_currTowerLevel].cost;
        _currTowerLevel++;
        PopulateInfo(towerTemplate);
        ShowRangeIndicator();
        _audioManager.Play("upgrade");
        return true;
    }

    public void DegradeTower()
    {
        if (_currTowerLevel <= 0)
        {
            _audioManager.Play("explosion");
            DestroyTower();
            return;
        }
        _currTowerLevel--;
        PopulateInfo(towerTemplate);
        ShowRangeIndicator();
        _audioManager.Play("degrade");
    }

    public void SellTower()
    {
        _audioManager.Play("sell");
        _gameManager.money += ReturnSellCost();
        DestroyTower();
    }

    public void DestroyTower()
    {
        Instantiate(_effectsManager.effects["explosion"], transform.position, Quaternion.identity);
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

    public void OnSelectTarget()
    {
        _spriteRend.material = _gameManager.pixelOutlineMat;
        ShowRangeIndicator();
    }

    public void OnDeselectTarget()
    {
        _spriteRend.material = _gameManager.defaultSpriteMat;
        HideRangeIndicator();
    }

    public int ReturnUpgradeCost()
    {
        return _currTowerLevel < towerTemplate.towerLevels.Length - 1 ? towerTemplate.towerLevels[_currTowerLevel + 1].cost : 0;
    }

    public int ReturnSellCost()
    {
        int sellCost = towerTemplate.towerLevels[0].cost / 2;
        for (int i = 0; i < _currTowerLevel; i++)
        {
            sellCost += towerTemplate.towerLevels[i].cost / 2;
        }
        return sellCost;
    }

    public TowerStage ReturnCurrentUpgrade()
    {
        return towerTemplate.towerLevels[_currTowerLevel];
    }

    public TowerStage ReturnNextUpgrade()
    {
        return _currTowerLevel < towerTemplate.towerLevels.Length - 1 ? towerTemplate.towerLevels[_currTowerLevel + 1] : null;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            DegradeTower();
    }

    public void PopulateInfo(TowerTemplate template)
    {
        towerTemplate = template;
        _spriteRend.sprite = towerTemplate.towerLevels[_currTowerLevel].sprite;
        name = towerTemplate.towerLevels[_currTowerLevel].name;
        health = towerTemplate.towerLevels[_currTowerLevel].health;
    }

    private void OnDrawGizmosSelected()
    {
        if (towerTemplate == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerTemplate.towerLevels[_currTowerLevel].range);
    }
}
