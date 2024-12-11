using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    
    public static AudioManager instance;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    
    const string masterVolume = "MasterVolume";
    const string sfxVolume = "SFXVolume";
    const string musicVolume = "MusicVolume";
    
    public float MasterVolume => PlayerPrefs.GetFloat(masterVolume, 1);
    public float SFXVolume => PlayerPrefs.GetFloat(sfxVolume, 1);
    public float MusicVolume => PlayerPrefs.GetFloat(musicVolume, 1);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        musicSource.clip = backgroundMusic;
        
        AudioListener.volume = MasterVolume;
        sfxSource.volume = SFXVolume;
        musicSource.volume = MusicVolume;
        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
    public void ChangeBackgroundMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    
    public void ChangeMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(masterVolume, volume);
    }
    
    public void ChangeSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat(sfxVolume, volume);
    }
    
    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat(musicVolume, volume);
    }
}
