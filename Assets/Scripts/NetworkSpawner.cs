using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [SerializeField] private List<GameObject> players;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        if (!IsHost) return;
        GridManager.Singleton.Initialize();
        
        // Vector with the position of the start room
        Vector3 startPosition = GridManager.Singleton.GetGrid().GetStartRoom().GetPosition() * GridManager.Singleton.SizeBetweenRooms + new Vector3(0,0.5f,0);
        Vector3 ghostPosition = new Vector3(Random.Range(0, GridManager.Singleton.GridWidth * GridManager.Singleton.SizeBetweenRooms),
            1.0f, Random.Range(2*GridManager.Singleton.SizeBetweenRooms, GridManager.Singleton.GridHeight * GridManager.Singleton.SizeBetweenRooms));


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
