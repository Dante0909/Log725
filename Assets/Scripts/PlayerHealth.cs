using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class PlayerHealth : NetworkBehaviour
{
    public static event Action<int> HealthChanged =  delegate{};

    public NetworkVariable<int> playerHealth = new NetworkVariable<int>();
    public AudioClip hitSound;
    public AudioClip healSound;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.OnValueChanged += OnPlayerHealthChanged;
        if(IsHost)
            playerHealth.Value = 3;
        
        OnPlayerHealthChanged(0, playerHealth.Value);

    }

    public void DecreaseHealth()
    {
        playerHealth.Value--;

        if (playerHealth.Value <= 0)
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

        playerHealth.Value++;
    }

    private void OnPlayerHealthChanged(int prevValue, int newValue)
    {
        HealthChanged?.Invoke(newValue);
    }
}