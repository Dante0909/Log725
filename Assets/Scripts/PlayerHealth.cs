using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> HealthChanged =  delegate{};

    public int playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 3;
    }

    public void DecreaseHealth()
    {
        playerHealth--;
        HealthChanged?.Invoke(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}