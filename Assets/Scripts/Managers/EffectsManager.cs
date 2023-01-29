using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Effect
{
    public string name;
    public GameObject prefab;
}

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private Effect[] availableEffect;
    [HideInInspector] public Dictionary<string, GameObject> effects;

    private void Awake()
    {
        effects = new Dictionary<string, GameObject>();
        foreach (Effect effect in availableEffect)
        {
            effects.Add(effect.name, effect.prefab);
        }
    }
}
