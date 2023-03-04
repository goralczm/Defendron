using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    [SerializeField] private string onDestroyEffect;

    [HideInInspector] public float damage;
    [HideInInspector] public float range;
    [HideInInspector] public EnemyAi target;

    public bool replicateEffect;

    public virtual void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);

        transform.localRotation = targetRot;

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
        float distanceBtwTarget = Vector2.Distance(transform.position, target.transform.position);
        
        if (distanceBtwTarget <= 0.05f)
        {
            DamageTarget();
        }
    }

    public virtual void PopulateInfo(EnemyAi newTarget, float newDamage, float newRange)
    {
        target = newTarget;
        damage = newDamage;
        range = newRange;
    }

    public virtual void DamageTarget()
    {
        if (target == null)
            return;

        target.TakeDamage(damage);
        DestroyBullet();
    }

    public virtual void DestroyBullet()
    {
        if (replicateEffect)
            Debug.Log("REPLICATE");

        EffectsManager.instance.PlayEffect("bullet_impact", transform.position);
        Destroy(gameObject);
    }
}
