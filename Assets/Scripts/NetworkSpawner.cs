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
        
        // Vector with the position of the start room
        Vector3 startPosition = GridManager.Instance.GetGrid().GetStartRoom().GetPosition() * 19 + new Vector3(0, 1.5f,0);
        Vector3 ghostPosition = new Vector3(Random.Range(0, 15 * 19), 1.0f, Random.Range(4*19, 15 * 19));


        GameObject ghostPlayer = (GameObject)Instantiate(players[1], ghostPosition, transform.rotation);
        NetworkObject netObj1 = ghostPlayer.GetComponent<NetworkObject>();
        ghostPlayer.SetActive(true);
        netObj1.Spawn();

        CustomNetworkManager.Singleton.GhostGameObject = ghostPlayer;

            GameObject mainPlayer = (GameObject)Instantiate(players[0], startPosition, transform.rotation);
        NetworkObject netObj2 = mainPlayer.GetComponent<NetworkObject>();
        mainPlayer.SetActive(true);
        netObj2.Spawn(true);

    }
}
