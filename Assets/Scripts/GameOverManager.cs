using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        // Load the main menu scene
        //SceneManager.LoadScene("MainMenu");
        CustomNetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
