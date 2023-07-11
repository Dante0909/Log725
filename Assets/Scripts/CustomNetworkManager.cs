using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CustomNetworkManager : NetworkManager
{
    public bool IsClientConnected;
    
    private void Start()
    {
        ConnectionNotificationManager.Singleton.OnClientConnectionNotification += OnClientConnected;
    }
    
    private void OnClientConnected(ulong clientId, ConnectionNotificationManager.ConnectionStatus connStatus)
    {
        IsClientConnected = connStatus == ConnectionNotificationManager.ConnectionStatus.Connected;

        if (IsClientConnected)
        {
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
        }
        else
        {
            GetComponent<NetworkObject>().RemoveOwnership();
        }
        
    }
}
