using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> players;
    
    public override void OnNetworkSpawn()
    {
        SpawnPlayers();
        base.OnNetworkSpawn();
    }

    private void SpawnPlayers()
    {
        if (!IsHost) return;
        
        GameObject ghostPlayer = (GameObject)Instantiate(players[1]);
        NetworkObject netObj1 = ghostPlayer.GetComponent<NetworkObject>();
        ghostPlayer.SetActive(true);
        netObj1.Spawn();

        CustomNetworkManager.Singleton.GhostGameObject = ghostPlayer;

            GameObject mainPlayer = (GameObject)Instantiate(players[0]);
        NetworkObject netObj2 = mainPlayer.GetComponent<NetworkObject>();
        mainPlayer.SetActive(true);
        netObj2.Spawn(true);

    }
}
