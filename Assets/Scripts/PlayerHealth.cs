using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> HealthChanged =  delegate{};

    private int playerHealth;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DecreaseHealth();
        }
    }
}