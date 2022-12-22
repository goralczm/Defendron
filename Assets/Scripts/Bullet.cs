using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float range;

    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, range);
        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].tag == "Enemy")
            {
                Destroy(hit[i].gameObject);
                Destroy(gameObject);
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
