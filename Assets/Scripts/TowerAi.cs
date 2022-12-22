using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAi : MonoBehaviour
{
    private Transform target;
    private float timer;

    public TowerTemplate towerTemplate;
    private int currTowerLevel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            UpgradeTower();

        if (target == null)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, towerTemplate.towerLevels[currTowerLevel].range);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].tag == "Enemy")
                {
                    if (target == null)
                        target = hit[i].transform;
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, target.position) > towerTemplate.towerLevels[currTowerLevel].range)
                target = null;
            else
            {
                if (timer <= 0)
                {
                    Vector3 diff = target.position - transform.position;
                    float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);
                    Instantiate(towerTemplate.towerLevels[currTowerLevel].bullet, transform.position, targetRot);
                    timer = towerTemplate.towerLevels[currTowerLevel].rateOfFire;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }

    public bool UpgradeTower()
    {
        if (currTowerLevel < towerTemplate.towerLevels.Length - 1)
        {
            currTowerLevel++;
            GetComponent<SpriteRenderer>().sprite = towerTemplate.towerLevels[currTowerLevel].sprite;
            name = towerTemplate.towerLevels[currTowerLevel].name;
            return true;
        }
        else
            return false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerTemplate.towerLevels[currTowerLevel].range);
    }
}
