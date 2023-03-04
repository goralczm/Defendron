using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    private Laser[] _laser;
    private float _cooldownTimer;

    public override void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        base.Update();
    }

    public override void NoTargets()
    {
        if (rateOfFire == 0)
            HideLasers();
    }

    public override void LimitStats()
    {
        rateOfFire = Mathf.Clamp(rateOfFire, 0, 3f);
        shootMultiplier = Mathf.Clamp(shootMultiplier, 1, 5);
    }

    public override void Shoot()
    {
        if (rateOfFire == 0)
        {
            for (int i = 0; i < _targets.Count; i++)
            {
                if (i == 5)
                    return;
                _laser[i].gameObject.SetActive(true);
                _laser[i].PopulateInfo(_targets[i].GetComponent<EnemyAi>(), damage, range);

                _laser[i].isMegaShot = false;
            }
        }
        else
        {
            if (_shootTimer > 0)
            {
                _laser[0].gameObject.SetActive(true);
                _laser[0].PopulateInfo(_targets[0].GetComponent<EnemyAi>(), 0, range);

                _laser[0].isMegaShot = true;
                _laser[0]._laser.widthMultiplier = 8;
                if (_targets[0] != null)
                    _targets[0].GetComponent<EnemyAi>().TakeDamage(damage);
                _shootTimer = 0;
            }
            else
            {
                if (_shootTimer <= -rateOfFire)
                {
                    _shootTimer = rateOfFire;
                }
            }
        }
    }

    public override bool UpgradeTower()
    {
        HideLasers();
        return base.UpgradeTower();
    }

    public override void DegradeTower()
    {
        HideLasers();
        base.DegradeTower();
    }

    public override void DestroyTower()
    {
        HideLasers();
        base.DestroyTower();
    }

    private void HideLasers()
    {
        for (int i = 0; i < _laser.Length; i++)
        {
            _laser[i].gameObject.SetActive(false);
        }
    }

    public override void ChangeProjectile(GameObject newLaser)
    {
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        _laser = new Laser[5];
        for (int i = 0; i < 5; i++)
        {
            _laser[i] = Instantiate(newLaser, transform.position, Quaternion.identity, transform).GetComponent<Laser>();
            _laser[i]._laser = _laser[i].GetComponent<LineRenderer>();
            _laser[i]._laser.positionCount = 2;
            _laser[i].gameObject.SetActive(false);
        }
    }

    public override string DisplayStats()
    {
        string newText = "Damage Per Tick \n " + damage + "\n" +
                            "Range \n " + range + "\n";

        return newText;
    }

    public override string DisplayUpgradeStats()
    {
        TowerLevel nextUpgrade = ReturnNextUpgrade();
        string newText = "Damage Per Tick \n " + damage + " > " + (damage - _currTowerLevel.damage + nextUpgrade.damage) + "\n" +
                            "Range \n " + range + " > " + (range - _currTowerLevel.range + nextUpgrade.range) + "\n";

        return newText;
    }
}
