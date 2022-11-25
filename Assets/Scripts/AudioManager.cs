using System;
using UnityEngine.Audio;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static Sound[] soundRecords;

    void Awake()
    {
        soundRecords = new Sound[sounds.Length];

        for (int i = 0; i < soundRecords.Length; i++)
        {
            soundRecords[i] = sounds[i];
            Sound s = sounds[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    // The following command is used to play an audio clip  
    // The common usage will look like the following:
    // AudioManager.Play("PlayerMove");

    public static void Play(string name)
    {
        Sound s;
        try
        {
            s = Array.Find(soundRecords, sound => sound.name == name);
            s.source.Play();
        }
        catch
        {
            Debug.Log("The sound: " + name + " does not exist.");
        }
    }
}