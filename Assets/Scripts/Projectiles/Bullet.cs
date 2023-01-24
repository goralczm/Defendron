using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject explodeEffect;
    [SerializeField] private float speed;

    [HideInInspector] public int damage;
    [HideInInspector] public EnemyAi target;

    public virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
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

    public virtual void DamageTarget()
    {
        target.TakeDamage(damage);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (explodeEffect != null)
            Instantiate(explodeEffect, transform.position, Quaternion.identity);
    }
}
