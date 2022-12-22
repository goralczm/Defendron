using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public GameObject bullet;
    public float rateOfFire;
    public float range;

    private Transform target;
    private float timer;

    private void Update()
    {
        if (target == null)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);
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
            if (Vector2.Distance(transform.position, target.position) > range)
                target = null;
            else
            {
                if (timer <= 0)
                {
                    Vector3 diff = target.position - transform.position;
                    float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                    Quaternion targetRot = Quaternion.Euler(0, 0, rotZ + 90f);
                    Instantiate(bullet, transform.position, targetRot);
                    timer = rateOfFire;
                }
                else
                {
                    timer -= Time.deltaTime;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
