using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private void Awake()
    {
        if (CustomNetworkManager.Singleton != null)
        {
            Destroy(CustomNetworkManager.Singleton.gameObject);
        }
    }

    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        // Load the main menu scene
        //SceneManager.LoadScene("MainMenu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
