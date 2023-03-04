using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform exitPortal;
    public Transform nextPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            EnemyAi hit = collision.GetComponent<EnemyAi>();
            collision.transform.position = exitPortal.position;
            hit.SetNextPoint(nextPoint);
        }
    }
}
