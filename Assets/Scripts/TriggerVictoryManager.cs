using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerVictoryManager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //SceneManager.LoadScene("EndPlayerWin");
            CustomNetworkManager.Singleton.SceneManager.LoadScene("EndPlayerWin", LoadSceneMode.Single);
        }
    }
}