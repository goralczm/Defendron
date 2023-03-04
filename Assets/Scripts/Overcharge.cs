using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overcharge : MonoBehaviour
{
    private Tower tower;
    private SpriteRenderer rend;
    private float overchargeMeter;
    private AudioManager _audio;

    private void Start()
    {
        _audio = GameManager.instance.GetComponent<AudioManager>();
        tower = GetComponent<Tower>();
        rend = GetComponent<SpriteRenderer>();
        tower.ShootHandler += OnShoot;
    }

    private void Update()
    {
        if (overchargeMeter > 0)
        {
            if (!tower.enabled)
                overchargeMeter -= Time.deltaTime * 10f;
            else
                overchargeMeter -= Time.deltaTime * 2.5f;
        }

        if (overchargeMeter <= 0 && !tower.enabled)
            tower.enabled = true;

        Color newColor = new Color(1, 1 - Mathf.Clamp(overchargeMeter / 25, 0, 1), 1 - Mathf.Clamp(overchargeMeter / 25, 0, 1));
        rend.color = newColor;
    }

    public void OnShoot()
    {
        overchargeMeter += 1;

        if (overchargeMeter >= 25)
        {
            tower.enabled = false;
            _audio.Play("overcharge");
        }
    }

    private void OnDestroy()
    {
        tower.ShootHandler -= OnShoot;
        rend.color = Color.white;
        tower.enabled = true;
    }
}
