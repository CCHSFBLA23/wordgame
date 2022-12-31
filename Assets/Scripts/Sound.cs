using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1.0f)]
    public float volume = 1f;
    [Range(0f, 5f)]
    public float pitch = 1f;

    public bool loop;
    [HideInInspector]
    public AudioSource source;
}