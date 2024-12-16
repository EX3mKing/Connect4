using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip clip;

    private void Start()
    {
        AudioManager.instance.ChangeBackgroundMusic(clip);
    }
}
