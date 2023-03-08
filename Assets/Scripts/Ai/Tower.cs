using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetSelectionOption { First, Last, Strongest }

public class Tower : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    [HideInInspector] public TargetSelectionOption howToSelectTarget;
    [HideInInspector] public bool isBuilding;

    public float damage;
    public float range;
    public float rateOfFire;
    public int shootMultiplier;
    public GameObject _projectile;

    public List<Module> upgrades;

    public SpriteRenderer _rangeIndicator;
    [HideInInspector] public SpriteRenderer _spriteRend;
    [HideInInspector] public GameManager _gameManager;
    [HideInInspector] public AudioManager _audioManager;
    [HideInInspector] public EffectsManager _effectsManager;
    [HideInInspector] public List<Transform> _targets;

    public List<Transform> _targetsToIngore;

    public float _shootTimer;
    private int _currLevelIndex;
    public TowerLevel _currTowerLevel;

    public bool paused;

    public delegate void ShootDelegate();
    public event ShootDelegate ShootHandler;

    public virtual void Awake()
    {
        //Instances
        _gameManager = GameManager.instance;
        _audioManager = _gameManager.GetComponent<AudioManager>();
        _effectsManager = _gameManager.GetComponent<EffectsManager>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _rangeIndicator = transform.GetChild(0).GetComponent<SpriteRenderer>();

        _targets = new List<Transform>();
        _targetsToIngore = new List<Transform>();
        upgrades = new List<Module>();
        ShootHandler += AfterShoot;
    }

    public virtual void Update()
    {
        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        for (int i = _targetsToIngore.Count - 1; i >= 0 ; i--)
        {
            if (_targetsToIngore[i] == null)
                _targetsToIngore.RemoveAt(i);
        }

        _shootTimer -= Time.deltaTime;

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

    public virtual void LimitStats()
    {
        rateOfFire = Mathf.Clamp(rateOfFire, 0.05f, 3f);
        shootMultiplier = Mathf.Clamp(shootMultiplier, 1, 5);
    }

    public virtual void Shoot()
    {
        if (_shootTimer <= 0)
        {
            List<Transform> targetsToDelete = new List<Transform>();
            foreach (Transform target in _targets)
            {
                Vector3 dir = target.transform.position - transform.position;
                float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

                Bullet tmpBulletScript = Instantiate(_projectile, transform.position, targetRot).GetComponent<Bullet>();
                EnemyAi currTarget = target.GetComponent<EnemyAi>();
                tmpBulletScript.PopulateInfo(currTarget, damage, range);
                _audioManager.Play("bullet");
            }
            _shootTimer = rateOfFire;

            ShootHandlerFunc();
        }
    }

    public void ShootHandlerFunc()
    {
        if (ShootHandler != null)
            ShootHandler();
    }

    public void AfterShoot()
    {

    }

    public void ResetTarget()
    {
        _targets.Clear();
    }

    #region Target Selection
    public void TargetSelection()
    {
        for (int i = _targets.Count - 1; i >= 0; i--)
        {
            if (_targets[i] == null)
                _targets.RemoveAt(i);
            else
            {
                float distance = Vector2.Distance(transform.position, _targets[i].position);
                if (distance > range)
                    _targets.RemoveAt(i);
            }
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, 1 << 8);

        List<Collider2D> validTargets = new List<Collider2D>();

        foreach (Collider2D hit in hits)
        {
            /*if (_targetsToIngore.Contains(hit.transform) || _targets.Contains(hit.transform))
                continue;*/

            Vector2 dir = hit.transform.position - transform.position;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, dir, range, (1 << 8) | (1 << 9));
            Debug.DrawRay(transform.position, dir.normalized * range, Color.green);

            if (ray.collider == hit)
                validTargets.Add(hit);
        }

        if (validTargets.Count == 0)
            return;

        EnemyAi[] targets = new EnemyAi[validTargets.Count];

        for (int i = 0; i < validTargets.Count; i++)
        {
            targets[i] = validTargets[i].GetComponent<EnemyAi>();
        }

        switch (howToSelectTarget)
        {
            case TargetSelectionOption.First:
                QuickSortByLenght(targets, 0, targets.Length - 1);
                for (int i = 1; i <= shootMultiplier - _targets.Count; i++)
                {
                    if (targets.Length - i >= 0)
                        _targets.Add(targets[targets.Length - i].transform);
                }
                break;
            case TargetSelectionOption.Last:
                QuickSortByLenght(targets, 0, targets.Length - 1);
                for (int i = 0; i < shootMultiplier - _targets.Count; i++)
                {
                    if (i <= targets.Length - 1)
                        _targets.Add(targets[i].transform);
                }
                break;
            case TargetSelectionOption.Strongest:
                QuickSortByStrenght(targets, 0, targets.Length - 1);

                if (targets[0].enemyTemplate.difficultyLevel == targets[targets.Length - 1].enemyTemplate.difficultyLevel)
                    QuickSortByLenght(targets, 0, targets.Length - 1);
                for (int i = 1; i <= shootMultiplier - _targets.Count; i++)
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

    public virtual void ChangeEnemyTargeting(bool next)
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
    #endregion

    public virtual bool UpgradeTower()
    {
        if (_currLevelIndex == towerTemplate.levels.Length - 1)
            return false;

        if (ReturnUpgradeCost() > _gameManager.money)
            return false;

        _currLevelIndex++;
        _currTowerLevel = towerTemplate.levels[_currLevelIndex];

        PopulateInfo(towerTemplate);
        UpdateRangeIndicator();

        _audioManager.Play("upgrade");

        return true;
    }

    public virtual void DegradeTower()
    {
        _currLevelIndex--;
        if (_currLevelIndex == 0)
        {
            _audioManager.Play("explosion");
            DestroyTower();
            return;
        }

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
        if (towerTemplate.onDestroyEffect != "")
            _effectsManager.PlayEffect(towerTemplate.onDestroyEffect, transform.position);
        _gameManager.ReleaseCell(this);
        Destroy(gameObject);
    }

    #region Range Indicator()
    public virtual void ShowRangeIndicator()
    {
        UpdateRangeIndicator();

        _rangeIndicator.gameObject.SetActive(true);
    }

    public virtual void HideRangeIndicator()
    {
        _rangeIndicator.gameObject.SetActive(false);
    }

    public virtual void UpdateRangeIndicator()
    {
        _rangeIndicator.transform.localScale = new Vector3(range, range, range);
    }
    #endregion

    public void OnSelectTarget()
    {
        ShowRangeIndicator();
    }

    public void OnDeselectTarget()
    {
        HideRangeIndicator();
    }

    public int ReturnUpgradeCost()
    {
        return _currLevelIndex < towerTemplate.levels.Length - 1 ? towerTemplate.levels[_currLevelIndex + 1].cost : 0;
    }

    public TowerLevel ReturnNextUpgrade()
    {
        return _currLevelIndex < towerTemplate.levels.Length - 1 ? towerTemplate.levels[_currLevelIndex + 1] : _currTowerLevel;
    }

    public int ReturnSellCost()
    {
        int sellCost = towerTemplate.levels[0].cost / 2;
        for (int i = 1; i < _currLevelIndex; i++)
        {
            sellCost += towerTemplate.levels[i].cost / 2;
        }

        return sellCost;
    }

    public void PopulateInfo(TowerTemplate template)
    {
        towerTemplate = template;
        _currTowerLevel = towerTemplate.levels[_currLevelIndex];

        bool foundModuleChangingProjectile = false;
        foreach (Module upgrade in upgrades)
        {
            if (upgrade.projectile != null)
                foundModuleChangingProjectile = true;
        }
        if (!foundModuleChangingProjectile)
            ChangeProjectile(_currTowerLevel.bullet);

        _spriteRend.sprite = _currTowerLevel.sprite;
        name = template.name + " Level " + _currLevelIndex;

        ResetStats();
    }

    public void ResetStats()
    {
        damage = _currTowerLevel.damage;
        range = _currTowerLevel.range;
        rateOfFire = _currTowerLevel.rateOfFire;
        shootMultiplier = _currTowerLevel.shootsMultiplier;

        RemountUpgrades();
    }

    public virtual void ChangeProjectile(GameObject newProjectile)
    {
        _projectile = newProjectile;
    }

    public virtual bool SwapModules(Module oldModule, Module newModule)
    {
        if (!upgrades.Contains(oldModule))
            return MountUpgrade(newModule);

        int indexOfOldModule = upgrades.IndexOf(oldModule);
        bool mounted = MountUpgrade(newModule, indexOfOldModule);

        if (!mounted)
            return false;

        DemountUpgrade(oldModule);
        if (newModule.projectile != null)
            ChangeProjectile(newModule.projectile);
        return true;
    }

    public virtual bool MountUpgrade(Module newUpgrade, int insertIndex = -2)
    {
        if (insertIndex >= 0)
            upgrades.Insert(insertIndex, newUpgrade);
        else
            upgrades.Add(newUpgrade);
        if (newUpgrade.script != "")
        {
            System.Type scriptType = System.Type.GetType(newUpgrade.script);
            gameObject.AddComponent(scriptType);
        }
        if (newUpgrade.projectile != null)
            ChangeProjectile(newUpgrade.projectile);
        ResetStats();
        UpdateRangeIndicator();
        return true;
    }

    public virtual void DemountUpgrade(Module upgrade)
    {
        upgrades.Remove(upgrade);
        if (upgrade.script != "")
        {
            MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                if (component.GetType().Name == upgrade.script)
                {
                    Destroy(component);
                    break;
                }
            }
        }
        bool foundModuleChangingProjectile = false;
        foreach (Module equipedUpgrades in upgrades)
        {
            if (equipedUpgrades.projectile != null)
                foundModuleChangingProjectile = true;
        }
        if (!foundModuleChangingProjectile)
            ChangeProjectile(_currTowerLevel.bullet);
        ResetStats();
        UpdateRangeIndicator();
    }

    public virtual void RemountUpgrades()
    {
        foreach (Module upgrade in upgrades)
        {
            damage += upgrade.damage;
            range += upgrade.range;
            rateOfFire += upgrade.rateOfFire;
            shootMultiplier += upgrade.shootMultiplier;
        }

        LimitStats();
    }

    public virtual string DisplayStats()
    {
        string newText = "Damage \n " + damage + "\n" +
                            "Range \n " + range + "\n" +
                            "Rate of Fire \n " + rateOfFire + "\n";

        return newText;
    }

    public virtual string DisplayUpgradeStats()
    {
        TowerLevel nextUpgrade = ReturnNextUpgrade();
        string newText = "Damage \n " + damage + " > " + (damage - _currTowerLevel.damage + nextUpgrade.damage) + "\n" +
                            "Range \n " + range + " > " + (range - _currTowerLevel.range + nextUpgrade.range) + "\n" +
                            "Rate of Fire \n " + rateOfFire + " > " + (rateOfFire - _currTowerLevel.rateOfFire + nextUpgrade.rateOfFire) + "\n";

        return newText;
    }

    private void OnDrawGizmosSelected()
    {
        if (towerTemplate == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
