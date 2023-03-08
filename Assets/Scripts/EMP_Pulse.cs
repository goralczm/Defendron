using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMP_Pulse : MonoBehaviour
{
    [Range(0.05f, 0.3f)] public float precision = 0.05f;
    [Range(0f, 2f)] public float cooldown = .5f;

    private SpriteRenderer pulse;
    private Tower parentTower;
    private float _timer;

    private void Start()
    {
        parentTower = GetComponent<Tower>();
        pulse = Instantiate(GameManager.instance.pulseEffect, transform.position, Quaternion.identity, transform).GetComponent<SpriteRenderer>();
        pulse.transform.localScale = Vector3.zero;
        _timer = cooldown;
    }

    private void Update()
    {
        
        pulse.transform.localScale = Vector3.Lerp(pulse.transform.localScale, Vector3.one * parentTower.range, Time.deltaTime * 10f);

        if (_timer < 0)
        {
            float diff = Vector3.Distance(pulse.transform.localScale, Vector3.one * parentTower.range);
            if (diff <= precision)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pulse.transform.localScale.x, 1 << 8);
                for (int i = 0; i < hits.Length; i++)
                {
                    EnemyAi hit = hits[i].GetComponent<EnemyAi>();
                    if (hit != null)
                        hit.TakeDamage(.5f);
                }
                pulse.transform.localScale = Vector3.zero;
                pulse.color = Color.white;
            }
            _timer = cooldown;
        }
        else
            _timer -= Time.deltaTime;

        if (pulse.transform.localScale.x >= parentTower.range * 0.75)
        {
            Color newColor = pulse.color;
            newColor.a -= Time.deltaTime * 4f;
            pulse.color = newColor;
        }
    }

    private void OnDestroy()
    {
        Destroy(pulse.gameObject);
    }
}
