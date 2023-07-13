using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientManager : NetworkBehaviour
{
    public static new ClientManager Singleton { get; internal set; }
    public NetworkVariable<bool> IsClientConnected = new NetworkVariable<bool>();
    
    private void Awake()
    {
        if (Singleton != null)
        {
            // As long as you aren't creating multiple NetworkManager instances, throw an exception.
            // (***the current position of the callstack will stop here***)
            throw new Exception($"Detected more than one instance of {nameof(ClientManager)}! " +
                $"Do you have more than one component attached to a {nameof(GameObject)}");
        }
        Singleton = this;
    }

}
