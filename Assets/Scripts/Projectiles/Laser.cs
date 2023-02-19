using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    private LineRenderer _laser;

    private void Start()
    {
        _laser = GetComponent<LineRenderer>();
        _laser.positionCount = 2;
    }

    public override void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }

        target.TakeDamage(damage * Time.deltaTime);

        _laser.SetPosition(0, transform.position);
        _laser.SetPosition(1, target.transform.position);
    }
}
