using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatRay : Laser
{
    public override void DealDamage(float newDamage)
    {
        base.DealDamage(newDamage);

        if (target == null)
            return;

        Fire fire = target.GetComponent<Fire>();

        if (fire == null)
        {
            System.Type scriptType = System.Type.GetType("Fire");
            target.gameObject.AddComponent(scriptType);
            return;
        }

        fire.timer = 5;
    }
}
