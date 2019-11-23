using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] effects;
    public Sound[] songs;

    public bool canPlayEffects = true;
    public bool canPlaySongs = true;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in effects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Sound s in songs)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        SoundData data = SaveSystem.LoadAudioSettings();
        if (data != null)
        {
            canPlaySongs = data.canPlaySongs;
            canPlayEffects = data.canPlayEffects;
        }
    }

    public void PlaySoundEffect(string name)
    {
        if (canPlayEffects)
        {
            Sound s = Array.Find(effects, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Play();
        }
    }

    public void PlaySong(string name)
    {
        if (canPlaySongs)
        {
            Sound s = Array.Find(songs, sound => sound.name == name);
            if (s == null)
                return;
            s.source.Play();
        }
    }

    public void ChangeEffect()
    {
        foreach (Sound s in effects)
        {
            s.source.mute = canPlayEffects;
        }
        canPlayEffects = !canPlayEffects;

        SaveSystem.SaveAudioSettings(this);
    }

    public void ChangeSongs()
    {
        foreach (Sound s in songs)
        {
            s.source.mute = canPlaySongs;
            if (s.source.isPlaying) 
                s.source.Play();
        }
        canPlaySongs = !canPlaySongs;

        SaveSystem.SaveAudioSettings(this);
    }

    public bool IsSongPlaying(string name)
    {
        Sound s = Array.Find(songs, sound => sound.name == name);
        if (s == null)
            return false;
        return s.source.isPlaying;
    }
}
