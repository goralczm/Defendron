using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultidirectionalTower : Tower
{
    private StaticLaser[] lasers;
    private float angle;

    public override void Awake()
    {
        base.Awake();

        lasers = new StaticLaser[transform.childCount - 1];
        for (int i = 1; i < transform.childCount; i++)
        {
            lasers[i - 1] = transform.GetChild(i).GetComponent<StaticLaser>();
        }

        UpdateRangeIndicator();
    }

    public override void Update()
    {
        if (isBuilding)
        {
            _rangeIndicator.color = _spriteRend.color;
            return;
        }

        if (paused)
        {
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].gameObject.SetActive(true);
            }
        }

        angle += Time.deltaTime * (1 + damage);
        float angleBetweenObjects = 2f * Mathf.PI / 4;
        for (int i = 0; i < lasers.Length; i++)
        {
            float objectAngle = i * angleBetweenObjects + angle;
            Vector3 position = new Vector3(Mathf.Cos(objectAngle), Mathf.Sin(objectAngle), 0) * range;
            position += transform.position;
            lasers[i].transform.GetChild(0).position = position;
        }
    }

    public override void RemountUpgrades()
    {
        base.RemountUpgrades();

        if (lasers == null)
            return;

        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].damage = damage;
            lasers[i].rateOfFire = rateOfFire;
            lasers[i].range = range;
        }
    }
}
