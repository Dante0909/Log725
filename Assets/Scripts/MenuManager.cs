using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Build number of scene to start when button is clicked
    public string gameStartScene;

    public void StartGame()
    {
        CustomNetworkManager.Singleton.StartHost();
        CustomNetworkManager.Singleton.SceneManager.LoadScene(gameStartScene, LoadSceneMode.Single);
    }

    public void JoinGame()
    {
        CustomNetworkManager.Singleton.StartClient();
        CustomNetworkManager.Singleton.SceneManager.LoadScene(gameStartScene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
