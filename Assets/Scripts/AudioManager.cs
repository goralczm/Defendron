using UnityEngine.Audio;
using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .2f;
    [Range(.1f, 3f)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}


public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup audioMixerGroup;
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = audioMixerGroup;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start()
    {
        //Play("Theme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
