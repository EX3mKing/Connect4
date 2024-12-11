using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class TestRelay : MonoBehaviour
{
    public static TestRelay Singleton { get; private set; }
    
    public TextMeshProUGUI joinCodeText;
    
    private void Awake()
    {
        Singleton = this;
    }
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        if (AuthenticationService.Instance.IsSignedIn) return;
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    
    public async void CreateRelay() // and start host
    {
        try
        { 
            // num of players limited to 2 (server + 1)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            NetworkManager.Singleton.StartHost();
            MultiplayerManager.Singleton.StartCheckingForSecondPlayer();
            joinCodeText.text = joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            throw;
        }
    }

    public async void JoinRelay(string code) // and start client
    {
        code = code.ToUpper();
        try
        {
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(code);
            joinCodeText.text = code;
            
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
            
            //MultiplayerManager.Singleton.JoinedRelayServerRpc();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            Debug.Log("Invalid join code or Room is full");
            throw;
        }
    }
}
