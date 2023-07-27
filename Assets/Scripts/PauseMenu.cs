using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : NetworkBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuCanvas;
    public GameObject optionMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsHost)
        {
            if (optionMenuCanvas.activeSelf)
            {
                optionMenuCanvas.SetActive(false);
            }

            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Resume the game
    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        //Time.timeScale = 1f;
        isPaused = false;
    }

    // Restart the game
    public void Restart()
    {
        pauseMenuCanvas.SetActive(false);
        //Time.timeScale = 1f;
        isPaused = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        RestartClientRpc();
        CustomNetworkManager.Singleton.Shutdown();
        Destroy(CustomNetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    // Pause the game
    void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        //Time.timeScale = 0f;
        isPaused = true;
    }

    // Load the main menu
    public void MainMenuButton()
    {
        //SceneManager.LoadScene("MainMenu");
        CustomNetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    
    [ClientRpc]
    private void RestartClientRpc()
    {
        if(!IsHost)
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
