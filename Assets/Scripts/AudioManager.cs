using System;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public AudioReferences references;
    public static Sound[] soundRecords;
    public List<AudioSource> musicSources = new List<AudioSource>();

    void Awake()
    {
        // Initialize Audio Clips
        soundRecords = new Sound[references.sounds.Count];
    
        for (int i = 0; i < soundRecords.Length; i++)
        {
            soundRecords[i] = references.sounds[i];
            Sound s = references.sounds[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            if (s.isMusic)
                musicSources.Add(s.source);
        }

        // Master Volume Stuff
        AudioListener.volume = SaveSystem.LoadOptionsData()?.masterVolume ?? 0.6f;
    }

    // The following command is used to play an audio clip  
    // The common usage will look like the following:
    // AudioManager.Play("PlayerMove");

    public static void Play(string title)
    {
        try
        {
            Sound s = Array.Find(soundRecords, sound => sound.name == title);
            OptionsData values = SaveSystem.LoadOptionsData();

            if (values != null)
            {
                if (s.isMusic)
                    s.source.volume = values.musicVolume;
                else
                    s.source.volume = values.effectsVolume;
            }
            else // default to exact masterVolume value
                s.source.volume = AudioListener.volume;

            s.source.Play();
        }
        catch
        {
            Debug.Log("The sound: " + title + " does not exist.");
        }
    }
}