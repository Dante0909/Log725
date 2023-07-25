using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> HealthChanged =  delegate{};

    public int playerHealth;
    public AudioClip hitSound;
    public AudioClip healSound;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 3;
    }

    public void DecreaseHealth()
    {
        playerHealth--;
        HealthChanged?.Invoke(playerHealth);

        if (playerHealth <= 0)
        {
            //SceneManager.LoadScene("EndGhostWin");
            CustomNetworkManager.Singleton.SceneManager.LoadScene("EndGhostWin", LoadSceneMode.Single);
        }
        else
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }
    }

    public void IncreaseHealth()
    {
        AudioSource.PlayClipAtPoint(healSound, transform.position);

        if (playerHealth < 3)
        {
            playerHealth++;
            HealthChanged?.Invoke(playerHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}