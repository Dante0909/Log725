using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : NetworkBehaviour
{
    private Vector3 moveVec;
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //moveVec = (playerTransform.position - transform.position).normalized;
        if (Input.GetKey(KeyCode.UpArrow))  
        {  
            moveVec = new Vector3(0, 0, 1);
        }  
         
        if (Input.GetKey(KeyCode.DownArrow))  
        {  
            moveVec = new Vector3(0, 0, -1);
        }  
         
        if (Input.GetKey(KeyCode.LeftArrow))  
        {  
            moveVec = new Vector3(-1, 0, 0);
        }  
        
        if (Input.GetKey(KeyCode.RightArrow))  
        {  
            moveVec = new Vector3(1, 0, 0);
        }  
        
        transform.Translate(moveVec * moveSpeed * Time.deltaTime);
        Position.Value = transform.position;

        moveVec = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DecreaseHealth();

            //to generate random
            transform.position = Vector3.zero;
        }
    }
}
