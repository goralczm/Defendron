using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : TowerAi
{
    private Laser _laser;

    public override void TargetOutOfRange()
    {
        _target = null;

        if (_laser == null)
            return;
        _laser.gameObject.SetActive(false);
    }

    public override void Shoot()
    {
        if (_laser == null)
            _laser = Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, Quaternion.identity).GetComponent<Laser>();
        _laser.gameObject.SetActive(true);
        _laser.PopulateInfo(_target.GetComponent<EnemyAi>(), towerTemplate.towerLevels[_currTowerLevel].damage, towerTemplate.towerLevels[_currTowerLevel].range);
    }

    public override bool UpgradeTower(bool pay)
    {
        if (_laser != null)
            DestroyLaser();
        return base.UpgradeTower(pay);
    }

    public override void DegradeTower()
    {
        if (_laser != null)
            DestroyLaser();
        base.DegradeTower();
    }

    public override void DestroyTower()
    {
        if (_laser != null)
            DestroyLaser();
        base.DestroyTower();
    }

    private void DestroyLaser()
    {
        if (_laser != null)
        {
            _laser.DestroyBullet();
            _laser = null;
        }
    }
}
