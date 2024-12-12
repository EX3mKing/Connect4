using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFunctions : MonoBehaviour
{
    [SerializeField] private AudioClip click;
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        if (startButton != null) MultiplayerManager.Singleton.foundSecondPlayer.AddListener(StartButtonFunction);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        
        Application.Quit();
    }
    
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadSceneAndDisconnect(int index)
    {
        MultiplayerManager.Singleton.DisconnectServerRpc(index);
    }
    
    public void Rematch()
    {
        MultiplayerManager.Singleton.RematchServerRpc();
    }
    
    // joins relay by taking a string from input field
    public void JoinRelay(TMP_InputField inputField)
    {
        TestRelay.Singleton.JoinRelay(inputField.text);
    }
    
    public void StartGame()
    {
        MultiplayerManager.Singleton.StartGameServerRpc();
    }
    
    public void BeginPlayFirst()
    {
        MultiplayerManager.Singleton.BeginPlayFirstServerRpc();
    }
    
    public void BeginPlaySecond()
    {
        MultiplayerManager.Singleton.BeginPlaySecondServerRpc();
    }
    
    public void BeginPlayRandom()
    {
        MultiplayerManager.Singleton.BeginPlayRandomServerRpc();
    }

    public void StartButtonFunction()
    {
        startButton.GetComponent<Button>().interactable = true;
        startButton.GetComponent<Image>().color = Color.white;
    }
    
    public void PlaySound()
    {
        AudioManager.instance.PlaySound(click);
    }
    
    public void ChangeMasterVolume(float volume)
    {
        AudioManager.instance.ChangeMasterVolume(volume);
    }
    
    public void ChangeSFXVolume(float volume)
    {
        AudioManager.instance.ChangeSFXVolume(volume);
    }
    
    public void ChangeMusicVolume(float volume)
    {
        AudioManager.instance.ChangeMusicVolume(volume);
    }
}
