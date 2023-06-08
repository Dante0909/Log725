using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        SceneManager.LoadScene("MainMenu");
    }
}
