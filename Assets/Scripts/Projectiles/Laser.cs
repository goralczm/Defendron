using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    [HideInInspector] public LineRenderer _laser;
    public bool isMegaShot = false;

    public override void Update()
    {
        if (isMegaShot)
        {
            _laser.widthMultiplier = Mathf.Lerp(_laser.widthMultiplier, 0, Time.deltaTime * 15f);
            DealDamage(0);
        }

        if (target == null)
        {
            if (!isMegaShot)
                gameObject.SetActive(false);
            return;
        }

        if (!isMegaShot)
        {
            _laser.widthMultiplier = Mathf.Clamp(damage / 2, 1, 4);
            DealDamage(damage);
        }


        _laser.SetPosition(0, transform.position);
        _laser.SetPosition(1, target.transform.position);
    }

    public virtual void DealDamage(float newDamage)
    {
        if (target == null)
            return;

        target.TakeDamage(newDamage / 60);
    }
}
