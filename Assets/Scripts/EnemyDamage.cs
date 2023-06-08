using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyDamage : NetworkBehaviour
{
    private PlayerHealth playerHealth;

    [SerializeField]
    private GameObject player;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.DecreaseHealth();

            //to generate random
            transform.position = Vector3.zero;
        }
    }
}
