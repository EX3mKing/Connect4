using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderSetter : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;
    
    private void Start()
    {
        masterSlider.value = AudioManager.instance.MasterVolume;
        sfxSlider.value = AudioManager.instance.SFXVolume;
        musicSlider.value = AudioManager.instance.MusicVolume;
    }
}
