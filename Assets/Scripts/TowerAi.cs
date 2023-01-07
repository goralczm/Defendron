using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAi : MonoBehaviour
{
    public TowerTemplate towerTemplate;

    private Transform _target;
    private float _timer;
    private int _currTowerLevel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UpgradeTower();

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

                    Instantiate(towerTemplate.towerLevels[_currTowerLevel].bullet, transform.position, targetRot);
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
            _currTowerLevel++;
            GetComponent<SpriteRenderer>().sprite = towerTemplate.towerLevels[_currTowerLevel].sprite;
            name = towerTemplate.towerLevels[_currTowerLevel].name;
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerTemplate.towerLevels[_currTowerLevel].range);
    }
}
