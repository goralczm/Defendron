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

    public static EffectsManager instance;

    private void Awake()
    {
        instance = this;

        effects = new Dictionary<string, GameObject>();
        foreach (Effect effect in availableEffect)
        {
            effects.Add(effect.name, effect.prefab);
        }
    }

    public void PlayEffect(string name, Vector3 position)
    {
        if (!effects.ContainsKey(name))
        {
            Debug.LogWarning("No effect " + name + " found!");
            return;
        }

        Instantiate(effects[name], position, Quaternion.identity);
    }
}
