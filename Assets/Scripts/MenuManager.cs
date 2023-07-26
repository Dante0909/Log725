using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Build number of scene to start when button is clicked
    public string gameStartScene;

    private void Awake()
    {
        if (CustomNetworkManager.Singleton != null)
        {
            Destroy(CustomNetworkManager.Singleton.gameObject);
        }
    }

    public void StartGame()
    {
        GameManager.IsHost = true;
        SceneManager.LoadScene(gameStartScene, LoadSceneMode.Single);
    }

    public void JoinGame()
    {
        GameManager.IsHost = false;
        SceneManager.LoadScene(gameStartScene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
