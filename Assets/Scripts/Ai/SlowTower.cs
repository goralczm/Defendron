using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    private List<EnemyAi> enemiesInRange;

    private void Start()
    {
        enemiesInRange = new List<EnemyAi>();
    }

    public override void Update()
    {
        if (_rangeIndicator != null)
            _rangeIndicator.transform.position = transform.position;

        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        SlowEnemies();
    }

    public override void ShowRangeIndicator()
    {
        base.ShowRangeIndicator();
        _rangeIndicator.color = new Color32(50, 125, 215, 150);
    }

    private void SlowEnemies()
    {
        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            enemiesInRange[i].currentSpeed = enemiesInRange[i].startSpeed;
        }
        enemiesInRange.Clear();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, 1 << 8);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyAi currEnemy = hits[i].GetComponent<EnemyAi>();
            currEnemy.currentSpeed = currEnemy.startSpeed * damage;
            enemiesInRange.Add(currEnemy);
        }
    }

    public override string DisplayStats()
    {
        string newText = "Slow Percent \n " + (damage * 100) + "\n" +
                            "Range \n " + range + "\n";

        return newText;
    }

    public override string DisplayUpgradeStats()
    {
        TowerLevel nextUpgrade = ReturnNextUpgrade();
        string newText = "Slow Percent \n " + (damage * 100) + " > " + (damage - _currTowerLevel.damage + nextUpgrade.damage) * 100 + "\n" +
                            "Range \n " + range + " > " + (range - _currTowerLevel.range + nextUpgrade.range) + "\n";

        return newText;
    }
}
