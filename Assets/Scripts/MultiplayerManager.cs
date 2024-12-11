using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{
    public static MultiplayerManager Singleton { get; private set; }

    public UnityEvent<int> input;
    public UnityEvent foundSecondPlayer;
    private GameManager gm;
    
    private void Awake()
    {
        // destroy if not the first instance
        if (Singleton != null && Singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        DontDestroyOnLoad(this);
        Singleton = this;
    }

    private void Update()
    {
        if (gm == null) return;
        if (!gm.CanDrop()) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MessageServerRpc(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MessageServerRpc(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MessageServerRpc(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MessageServerRpc(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            MessageServerRpc(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            MessageServerRpc(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            MessageServerRpc(7);
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        if (NetworkManager.Singleton == null) return;

        if (NetworkManager.Singleton.ConnectedClientsList.Count == 2)
        {
            StartGameClientRpc();
        }
    }
    
    [ClientRpc]
    private void StartGameClientRpc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(FindGM());
        Debug.Log("Starting Game...");
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void RematchServerRpc()
    {
        RematchClientRpc();
    }
    
    [ClientRpc]
    private void RematchClientRpc()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(FindGM());
    }

    [ServerRpc(RequireOwnership = false)]
    private void MessageServerRpc(int column)
    {
        MessageClientRpc(column);
    }
    
    [ClientRpc]
    private void MessageClientRpc(int column)
    {
   
        input.Invoke(column);
    }

    public void StartCheckingForSecondPlayer()
    {
        StartCoroutine(CheckForSecondPlayer());
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void DisconnectServerRpc(int index)
    {
        DisconnectClientRpc(index);
    }
    
    [ClientRpc]
    private void DisconnectClientRpc(int index)
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
        Destroy(gameObject);
        SceneManager.LoadScene(index);
    }
    
    private IEnumerator CheckForSecondPlayer()
    {
        Debug.Log("Started looking...");
        yield return new WaitForSeconds(3f);
        while (NetworkManager.Singleton.ConnectedClientsList.Count < 2)
        {
            yield return new WaitForSeconds(0.3f);
        }
        Debug.Log("Found Second Player!");
        foundSecondPlayer.Invoke();
    }
    
    private IEnumerator FindGM()
    {
        yield return new WaitForSeconds(0.1f);
        if (gm != null)
        {
            var a = FindObjectOfType<GameManager>();
            if (gm != a) gm = a;
        }
        else
        {
            gm = FindObjectOfType<GameManager>();
            StartCoroutine(FindGM());
        }
    }

    private void OnApplicationQuit()
    {
        DisconnectServerRpc(0);
    }
}
