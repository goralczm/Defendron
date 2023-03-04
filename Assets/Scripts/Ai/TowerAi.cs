/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum TargetSelectionOption { First, Last, Strongest }

public class TowerAi : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    public GameObject onDestroyEffect;
    public TargetSelectionOption howToSelectTarget;
    [HideInInspector] public bool isBuilding;

    public int health;
    public float damage;
    public float range;
    public float rateOfFire;
    public int shootMultiplier;

    public Transform healthBar;

    public List<Upgrade> upgrades;

    public SpriteRenderer _rangeIndicator;
    public SpriteRenderer _spriteRend;
    public GameManager _gameManager;
    public AudioManager _audioManager;
    public EffectsManager _effectsManager;
    public List<Transform> _targets;

    public float _timer;
    public int _currTowerLevel;

    public void Awake()
    {
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _effectsManager = _gameManager.GetComponent<EffectsManager>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _targets = new List<Transform>();
        upgrades = new List<Upgrade>();
        _rangeIndicator = transform.GetChild(1).GetComponent<SpriteRenderer>();
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

        _timer -= Time.deltaTime;

        float currentHealth = (float)health / (float)towerTemplate.towerLevels[_currTowerLevel].health * .8f;
        healthBar.localScale = new Vector3(currentHealth, healthBar.localScale.y, 1);

        TargetSelection();

        if (_targets.Count == 0)
        {
            NoTargets();
            return;
        }

        Shoot();
    }

    public virtual void NoTargets()
    {

    }

    public virtual void Shoot()
    {
        if (_timer <= 0)
        {
            foreach (Transform target in _targets)
            {
                {
                    Vector3 dir = target.transform.position - transform.position;
                    float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

                    //Bullet tmpBulletScript = Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, targetRot).GetComponent<Bullet>();
                    //tmpBulletScript.PopulateInfo(target.GetComponent<EnemyAi>(), towerTemplate.towerLevels[_currTowerLevel].damage, towerTemplate.towerLevels[_currTowerLevel].range);
                    _audioManager.Play("bullet");
                }
            }
            //_timer = towerTemplate.towerLevels[_currTowerLevel].rateOfFire;
        }
    }

    public void TargetSelection()
    {
        _targets.Clear();
        //Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerTemplate.towerLevels[_currTowerLevel].range, 1 << 8);

        if (hits.Length == 0)
            return;

        EnemyAi[] targets = new EnemyAi[hits.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            targets[i] = hits[i].GetComponent<EnemyAi>();
        }

        switch (howToSelectTarget)
        {
            case TargetSelectionOption.First:
                QuickSortByLenght(targets, 0, targets.Length - 1);
                for (int i = 1; i <= shootMultiplier; i++)
                {
                    if (targets.Length - i >= 0)
                        _targets.Add(targets[targets.Length - i].transform);
                }
                break;
            case TargetSelectionOption.Last:
                QuickSortByLenght(targets, 0, targets.Length - 1);
                for (int i = 0; i < shootMultiplier; i++)
                {
                    if (i <= targets.Length - 1)
                        _targets.Add(targets[i].transform);
                }
                break;
            case TargetSelectionOption.Strongest:
                QuickSortByStrenght(targets, 0, targets.Length - 1);
                for (int i = 1; i <= shootMultiplier; i++)
                {
                    if (targets.Length - i >= 0)
                        _targets.Add(targets[targets.Length - i].transform);
                }
                break;
        }
    }

    #region QuickSort By Lenght
    private void QuickSortByLenght(EnemyAi[] targets, int left, int right)
    {
        if (left >= right) return; // base case: array is sorted

        float pivot = MedianOfThree(targets, left, right);
        int index = Partition(targets, left, right, pivot);
        QuickSortByLenght(targets, left, index - 1);
        QuickSortByLenght(targets, index, right);
    }

    private int Partition(EnemyAi[] targets, int left, int right, float pivot)
    {
        while (left <= right)
        {
            while (targets[left].howFar < pivot) left++;
            while (targets[right].howFar > pivot) right--;

            if (left <= right)
            {
                SwapElements(targets, left, right);
                left++;
                right--;
            }
        }

        return left;
    }

    private float MedianOfThree(EnemyAi[] arr, int left, int right)
    {
        int mid = left + (right - left) / 2;
        if (arr[left].howFar > arr[mid].howFar) SwapElements(arr, left, mid);
        if (arr[left].howFar > arr[right].howFar) SwapElements(arr, left, right);
        if (arr[mid].howFar > arr[right].howFar) SwapElements(arr, mid, right);
        return arr[mid].howFar;
    }
    #endregion

    #region QuickSort By Strenght
    private void QuickSortByStrenght(EnemyAi[] targets, int left, int right)
    {
        if (left >= right) return; // base case: array is sorted

        float pivot = MedianOfThreeStrenght(targets, left, right);
        int index = PartitionStrenght(targets, left, right, pivot);
        QuickSortByStrenght(targets, left, index - 1);
        QuickSortByStrenght(targets, index, right);
    }

    private int PartitionStrenght(EnemyAi[] targets, int left, int right, float pivot)
    {
        while (left <= right)
        {
            while (targets[left].enemyTemplate.difficultyLevel < pivot) left++;
            while (targets[right].enemyTemplate.difficultyLevel > pivot) right--;

            if (left <= right)
            {
                SwapElements(targets, left, right);
                left++;
                right--;
            }
        }

        return left;
    }

    private float MedianOfThreeStrenght(EnemyAi[] arr, int left, int right)
    {
        int mid = left + (right - left) / 2;
        if (arr[left].enemyTemplate.difficultyLevel > arr[mid].enemyTemplate.difficultyLevel) SwapElements(arr, left, mid);
        if (arr[left].enemyTemplate.difficultyLevel > arr[right].enemyTemplate.difficultyLevel) SwapElements(arr, left, right);
        if (arr[mid].enemyTemplate.difficultyLevel > arr[right].enemyTemplate.difficultyLevel) SwapElements(arr, mid, right);
        return arr[mid].enemyTemplate.difficultyLevel;
    }
    #endregion

    private void SwapElements(EnemyAi[] arr, int firstIndex, int secondIndex)
    {
        EnemyAi tmpEl = arr[firstIndex];
        arr[firstIndex] = arr[secondIndex];
        arr[secondIndex] = tmpEl;
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
        //if (_currTowerLevel == towerTemplate.towerLevels.Length - 1)
            //return false;

        if (ReturnUpgradeCost() > _gameManager.money && pay)
            return false;

        if (pay)
            _gameManager.money -= ReturnUpgradeCost();
        _currTowerLevel++;
        PopulateInfo(towerTemplate);
        UpdateRangeIndicator();
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
        UpdateRangeIndicator();
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
        UpdateRangeIndicator();

        _rangeIndicator.gameObject.SetActive(true);
    }

    public virtual void HideRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(false);
    }

    public void UpdateRangeIndicator()
    {
        _rangeIndicator.transform.localScale = new Vector3(range, range, range);
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
        //return _currTowerLevel < towerTemplate.towerLevels.Length - 1 ? towerTemplate.towerLevels[_currTowerLevel + 1].cost : 0;
        return 0;
    }

    public int ReturnSellCost()
    {
        //int sellCost = towerTemplate.towerLevels[0].cost / 2;
        for (int i = 0; i < _currTowerLevel; i++)
        {
        //    sellCost += towerTemplate.towerLevels[i].cost / 2;
        }
        //return sellCost;
        return 0;
    }

    *//*public TowerStage ReturnCurrentTowerLevel()
    {
        //return towerTemplate.towerLevels[_currTowerLevel];
        return null;
    }

    public TowerStage ReturnNextUpgrade()
    {
        return _currTowerLevel < towerTemplate.towerLevels.Length - 1 ? towerTemplate.towerLevels[_currTowerLevel + 1] : null;
    }*//*

    public void PopulateInfo(TowerTemplate template)
    {
        towerTemplate = template;

        TowerStage currentStage = ReturnCurrentTowerLevel();
        _spriteRend.sprite = currentStage.sprite;
        name = template.name;

        ResetStats();
    }

    public void ResetStats()
    {
        TowerStage currentStage = ReturnCurrentTowerLevel();

        health = currentStage.health;
        damage = currentStage.damage;
        range = currentStage.range;
        rateOfFire = currentStage.rateOfFire;
        shootMultiplier = currentStage.shootsMultiplier;

        RemountUpgrades();
    }

    public void MountUpgrade(Upgrade newUpgrade)
    {
        upgrades.Add(newUpgrade);
        ResetStats();
        UpdateRangeIndicator();
    }

    public void DemountUpgrade(Upgrade upgrade)
    {
        upgrades.Remove(upgrade);
        ResetStats();
        UpdateRangeIndicator();
    }

    public void RemountUpgrades()
    {
        foreach (Upgrade upgrade in upgrades)
        {
            health += upgrade.health;
            damage += upgrade.damage;
            range += upgrade.range;
            rateOfFire += upgrade.rateOfFire;
            shootMultiplier += upgrade.shootMultiplier;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            DegradeTower();
    }

    public virtual string DisplayStats()
    {
        return "Not set yet";
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
*/