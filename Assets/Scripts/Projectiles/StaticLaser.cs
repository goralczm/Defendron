using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaser : Laser
{
    public Transform laserEnd;

    public float rateOfFire;
    private float targetWidth;
    private float _timer;

    private void Awake()
    {
        _laser = GetComponent<LineRenderer>();
        _laser.widthMultiplier = 0;
        targetWidth = 1.5f;
    }

    public override void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
        {
            targetWidth = 0;
            if (_timer < -rateOfFire / 2)
            {
                _timer = rateOfFire;
                targetWidth = 1.5f;
            }
        }

        _laser.SetPosition(0, transform.position);
        _laser.SetPosition(1, laserEnd.position);
        _laser.widthMultiplier = Mathf.Lerp(_laser.widthMultiplier, targetWidth, Time.deltaTime * 10f);

        if (_laser.widthMultiplier <= 0.3f)
            return;

        Vector2 dir = laserEnd.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, range, 1 << 8);

        if (hit.collider != null)
        {
            hit.transform.GetComponent<EnemyAi>().TakeDamage(damage / 60);
        }
    }
}
