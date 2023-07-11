using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyController : NetworkBehaviour
{
    private Vector3 moveVec;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Transform playerTransform;
    private Rigidbody rb;

    private bool isClientConnected;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        ConnectionNotificationManager.Singleton.OnClientConnectionNotification += OnClientConnected;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

        if (!isClientConnected)
        {
            moveVec = (playerTransform.position - transform.position).normalized;
        }
        
        rb.velocity = moveVec * moveSpeed * Time.fixedDeltaTime;
    }

    public void OnMoveGhost(InputValue input)
    {
        if (!IsOwner || !isClientConnected) return;
        Vector2 inputVec = input.Get<Vector2>();

        moveVec = new Vector3(inputVec.x, 0, inputVec.y);
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

    private void OnClientConnected(ulong clientId, ConnectionNotificationManager.ConnectionStatus connStatus)
    {
        isClientConnected = connStatus == ConnectionNotificationManager.ConnectionStatus.Connected;
        moveVec = Vector3.zero;

        if (isClientConnected)
        {
            GetComponent<NetworkObject>().ChangeOwnership(clientId);
            moveVec = Vector3.zero;
        }
        else
        {
            GetComponent<NetworkObject>().RemoveOwnership();
        }
        
    }
}
