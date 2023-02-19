using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostTower : TowerAi
{
    private List<EnemyAi> enemiesInRange;

    public override void Start()
    {
        base.Start();
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

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, towerTemplate.towerLevels[_currTowerLevel].range, 1 << 8);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyAi currEnemy = hits[i].GetComponent<EnemyAi>();
            currEnemy.currentSpeed = currEnemy.startSpeed * towerTemplate.towerLevels[_currTowerLevel].damage;
            enemiesInRange.Add(currEnemy);
        }
    }
}
