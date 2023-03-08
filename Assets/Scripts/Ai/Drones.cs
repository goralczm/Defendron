using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones : Tower
{
    public Tower[] drones;

    private float speed = 1f;
    private float angle;
    private Transform pathIndicator;

    public override void Awake()
    {
        base.Awake();

        if (transform.childCount < 1)
            return;

        drones = new Tower[transform.childCount - 2];
        for (int i = 1; i < transform.childCount - 1; i++)
        {
            drones[i - 1] = transform.GetChild(i).GetComponent<Tower>();
        }
        //pathIndicator = transform.GetChild(transform.childCount - 1);
    }

    public override void Update()
    {
        /*pathIndicator.localScale = _rangeIndicator.transform.localScale;
        pathIndicator.gameObject.SetActive(!_rangeIndicator.gameObject.activeSelf);*/

        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        if (!isBuilding)
        {
            angle += speed * Time.deltaTime;

            float angleBetweenObjects = 2f * Mathf.PI / shootMultiplier;

            for (int i = 0; i < drones.Length; i++)
            {
                if (shootMultiplier <= i)
                {
                    drones[i].transform.position = Vector3.Lerp(drones[i].transform.position, transform.position, Time.deltaTime * 40f);
                    float distanceFromCenter = Vector2.Distance(drones[i].transform.position, transform.position);
                    if (distanceFromCenter <= 0.3f)
                        drones[i].gameObject.SetActive(false);
                    continue;
                }

                drones[i].gameObject.SetActive(true);

                float objectAngle = i * angleBetweenObjects + angle;
                Vector3 position = new Vector3(Mathf.Cos(objectAngle), Mathf.Sin(objectAngle), 0) * range;
                position += transform.position;

                drones[i].transform.position = Vector3.Lerp(drones[i].transform.position, position, Time.deltaTime * 20f);
            }
        }
    }

    public override void RemountUpgrades()
    {
        base.RemountUpgrades();

        foreach (Tower drone in drones)
        {
            drone.damage = damage;
            drone.range = range / 2;
            drone.rateOfFire = rateOfFire;
            drone.shootMultiplier = 1;
        }
    }

    public override void ChangeEnemyTargeting(bool next)
    {
        base.ChangeEnemyTargeting(next);

        foreach (Tower drone in drones)
        {
            drone.ChangeEnemyTargeting(next);
        }
    }

    #region Range Indicator()
    public override void ShowRangeIndicator()
    {
        base.ShowRangeIndicator();
        if (drones != null)
        {
            foreach (Tower drone in drones)
            {
                drone.ShowRangeIndicator();
            }
        }
    }

    public override void HideRangeIndicator()
    {
        if (drones != null)
        {
            foreach (Tower drone in drones)
            {
                drone.HideRangeIndicator();
            }
        }
        base.HideRangeIndicator();
    }

    public override void UpdateRangeIndicator()
    {
        if (drones != null)
        {
            foreach (Tower drone in drones)
            {
                drone.UpdateRangeIndicator();
            }
        }
        base.UpdateRangeIndicator();
    }
    #endregion
}
