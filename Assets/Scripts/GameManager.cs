using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsHost;
    
    private void Start()
    {
        if (IsHost)
            CustomNetworkManager.Singleton.StartHost();
        else
            CustomNetworkManager.Singleton.StartClient();
    }
}
