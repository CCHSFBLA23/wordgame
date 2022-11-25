using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1.0f)]
    public float volume;
    [Range(0f, 100f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}