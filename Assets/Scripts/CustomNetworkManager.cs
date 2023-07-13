using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public static new CustomNetworkManager Singleton { get; internal set; }
    
    public GameObject GhostGameObject;
    
    private void Awake()
    {
        
        if (Singleton != null)
        {
            // As long as you aren't creating multiple NetworkManager instances, throw an exception.
            // (***the current position of the callstack will stop here***)
            throw new Exception($"Detected more than one instance of {nameof(CustomNetworkManager)}! " +
                                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }
    
    private void Start()
    {
        ConnectionNotificationManager.Singleton.OnClientConnectionNotification += OnClientConnected;
    }
    
    private void OnClientConnected(ulong clientId, ConnectionNotificationManager.ConnectionStatus connStatus)
    {
        if (!IsHost || ConnectedClientsIds.Count <= 1) return;
        ClientManager.Singleton.IsClientConnected.Value = connStatus == ConnectionNotificationManager.ConnectionStatus.Connected;

        if (ClientManager.Singleton.IsClientConnected.Value)
        {
            GhostGameObject.GetComponent<NetworkObject>().ChangeOwnership(clientId);
        }
        else
        {
            GhostGameObject.GetComponent<NetworkObject>().RemoveOwnership();
        }
        
    }
}
