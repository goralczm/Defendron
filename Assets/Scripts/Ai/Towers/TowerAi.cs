using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetSelectionOption { First, Last, Strongest }

public class TowerAi : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    public GameObject onDestroyEffect;
    public TargetSelectionOption howToSelectTarget;
    [HideInInspector] public bool isBuilding;

    public int health;
    public Transform healthBar;

    public SpriteRenderer _rangeIndicator;
    public SpriteRenderer _spriteRend;
    public GameManager _gameManager;
    public AudioManager _audioManager;
    public EffectsManager _effectsManager;
    public Transform _lastTarget;
    public Transform _target;

    public float _timer;
    public int _currTowerLevel;

    public void Awake()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _effectsManager = _gameManager.GetComponent<EffectsManager>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    public virtual void Start()
    {
        healthBar = transform.GetChild(0).transform;
    }

    public virtual void Update()
    {
        if (_rangeIndicator != null)
            _rangeIndicator.transform.position = transform.position;

        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        float currentHealth = (float)health / (float)towerTemplate.towerLevels[_currTowerLevel].health * .8f;
        healthBar.localScale = new Vector2(currentHealth, healthBar.localScale.y);

        TargetSelection(howToSelectTarget);

        if (_target == null)
            return;

        float distanceBtwTarget = Vector2.Distance(transform.position, _target.position);
        if (distanceBtwTarget > towerTemplate.towerLevels[_currTowerLevel].range)
        {
            TargetOutOfRange();
            return;
        }

        Shoot();
        _timer -= Time.deltaTime;
    }

    public virtual void Shoot()
    {
        if (_timer <= 0)
        {
            Vector3 dir = _target.transform.position - transform.position;
            float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

            Bullet tmpBulletScript = Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, targetRot).GetComponent<Bullet>();
            tmpBulletScript.PopulateInfo(_target.GetComponent<EnemyAi>(), towerTemplate.towerLevels[_currTowerLevel].damage, towerTemplate.towerLevels[_currTowerLevel].range);
            _audioManager.Play("bullet");
            _timer = towerTemplate.towerLevels[_currTowerLevel].rateOfFire;
        }
    }

    public virtual void TargetOutOfRange()
    {
        _target = null;
    }

    public void TargetSelection(TargetSelectionOption howToSelect)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerTemplate.towerLevels[_currTowerLevel].range, 1 << 8);
        if (hits.Length <= 0)
            return;

        if (hits.Length == 1)
        {
            _lastTarget = _target;
            _target = hits[0].transform;
        }

        EnemyAi bestTarget = _target == null ? hits[0].GetComponent<EnemyAi>() : _target.GetComponent<EnemyAi>();
        switch (howToSelect)
        {
            case TargetSelectionOption.First:
                for (int i = 1; i < hits.Length; i++)
                {
                    EnemyAi currTarget = hits[i].GetComponent<EnemyAi>();
                    if (currTarget.howFar > bestTarget.howFar)
                        bestTarget = currTarget;
                }
                break;
            case TargetSelectionOption.Last:
                for (int i = 1; i < hits.Length; i++)
                {
                    EnemyAi currTarget = hits[i].GetComponent<EnemyAi>();
                    if (currTarget.howFar < bestTarget.howFar)
                        bestTarget = currTarget;
                }
                break;
            case TargetSelectionOption.Strongest:
                for (int i = 1; i < hits.Length; i++)
                {
                    EnemyAi currTarget = hits[i].GetComponent<EnemyAi>();
                    if (currTarget.enemyTemplate.difficultyLevel > bestTarget.enemyTemplate.difficultyLevel)
                        bestTarget = currTarget;
                }
                break;
        }
        _lastTarget = _target;
        _target = bestTarget.transform;
    }

    public void ChangeEnemyTargeting(bool next)
    {
        var values = Enum.GetValues(typeof(TargetSelectionOption));
        var index = Array.IndexOf(values, howToSelectTarget);

        if (next)
        {
            if (index == values.Length - 1)
            {
                howToSelectTarget = (TargetSelectionOption)values.GetValue(0);
                return;
            }
            howToSelectTarget = (TargetSelectionOption)values.GetValue(index + 1);
            return;
        }

        if (index == 0)
        {
            howToSelectTarget = (TargetSelectionOption)values.GetValue(values.Length - 1);
            return;
        }
        howToSelectTarget = (TargetSelectionOption)values.GetValue(index - 1);
    }

    public virtual bool UpgradeTower(bool pay)
    {
        if (_currTowerLevel == towerTemplate.towerLevels.Length - 1)
            return false;

        if (ReturnUpgradeCost() > _gameManager.money && pay)
            return false;

        if (pay)
            _gameManager.money -= ReturnUpgradeCost();
        _currTowerLevel++;
        PopulateInfo(towerTemplate);
        _audioManager.Play("upgrade");
        return true;
    }

    public virtual void DegradeTower()
    {
        if (_currTowerLevel <= 0)
        {
            _audioManager.Play("explosion");
            DestroyTower();
            return;
        }
        _currTowerLevel--;
        PopulateInfo(towerTemplate);
        _audioManager.Play("degrade");
    }

    public void SellTower()
    {
        _audioManager.Play("sell");
        _gameManager.money += ReturnSellCost();
        DestroyTower();
    }

    public virtual void DestroyTower()
    {
        Instantiate(_effectsManager.effects["explosion"], transform.position, Quaternion.identity);
        HideRangeIndicator();
        Destroy(gameObject);
    }

    public virtual void ShowRangeIndicator()
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
    public virtual void HideRangeIndicator()
    {
        if (_rangeIndicator != null)
        {
            Destroy(_rangeIndicator.gameObject);
            _rangeIndicator = null;
        }
    }

    public void OnSelectTarget()
    {
        //_spriteRend.material = _gameManager.pixelOutlineMat;
        ShowRangeIndicator();
    }

    public void OnDeselectTarget()
    {
        //_spriteRend.material = _gameManager.defaultSpriteMat;
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

    public virtual void DisplayStats()
    {

    }

    private void OnDrawGizmosSelected()
    {
        if (towerTemplate == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerTemplate.towerLevels[_currTowerLevel].range);
    }

    private void OnMouseEnter()
    {
        healthBar.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        healthBar.gameObject.SetActive(false);
    }
}
