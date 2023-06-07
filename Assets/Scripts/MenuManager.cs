using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Build number of scene to start when button is clicked
    public int gameStartScene;

    public void StartGame()
    {
        SceneManager.LoadScene(gameStartScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
