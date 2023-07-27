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
    private float rotationSpeed;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private GameObject camera;
    private Rigidbody rb;
    private AiControllerState aiControllerState;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        aiControllerState = GetComponent<AiControllerState>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        if(!IsHost)
            camera.SetActive(true);
        else
            camera.SetActive(false);
        ConnectionNotificationManager.Singleton.OnClientConnectionNotification += OnClientConnected;
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = transform.position + new Vector3(0, 7f, -3.5f);
    }

    private void FixedUpdate()
    {
        //Debug.Log("\nIsClientConnected : " + ClientManager.Singleton.IsClientConnected.Value + " IsHost : " + IsHost);
        if (!ClientManager.Singleton.IsClientConnected.Value && IsHost)
        {
            moveVec = (playerTransform.position - transform.position).normalized;

            //Vector3 mv = aiControllerState?.GetMoveVec() ?? Vector3.zero;
            //moveVec = mv;
            Debug.Log("\nmoveVec : " + moveVec);
        }
        
        if(Vector3.Distance(playerTransform.position, transform.position) > 5f/*2f * GridManager.Singleton.SizeBetweenRoom*/){
            HandleAnimationServerRpc(0.0f);
        } else {
            Debug.Log("found player");
            HandleAnimationServerRpc(1.0f);
        }

        if(moveVec != Vector3.zero){
            Quaternion toRotate = Quaternion.LookRotation(moveVec, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        } else {
        }

        rb.velocity = moveVec * moveSpeed * Time.fixedDeltaTime;
    }

    public void OnMoveGhost(InputValue input)
    {
        //Debug.Log("\nOwnerClientID during OnMove" + OwnerClientId);
        //Debug.Log("\nIsOwner : " + IsOwner);
        //Debug.Log("\nClient connected : " + ClientManager.Singleton.IsClientConnected.Value);
        
        if (!IsOwner || !ClientManager.Singleton.IsClientConnected.Value) return;
        Debug.Log("Got here");
        Vector2 inputVec = input.Get<Vector2>();
        MoveServerRpc(inputVec.x, 0, inputVec.y);
        //Debug.Log("\nmoveVec : " + moveVec);
    }

    [ServerRpc]
    private void MoveServerRpc(float inputx, float inputy, float inputz)
    {
        Debug.Log("\nGot Here");

        moveVec = new Vector3(inputx, inputy, inputz);
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleAnimationServerRpc(float speed){
        if(animator != null)
            animator.SetFloat("Speed", speed);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.DecreaseHealth();

            // respawn at random position
            transform.position = new Vector3(Random.Range(0, GridManager.Singleton.SizeBetweenRooms * GridManager.Singleton.GridWidth),
                1.0f, Random.Range(0, GridManager.Singleton.SizeBetweenRooms * GridManager.Singleton.GridHeight));
        }
    }

    private void OnClientConnected(ulong clientId, ConnectionNotificationManager.ConnectionStatus connStatus)
    {
        //Debug.Log("\nOwnerClientID : " + OwnerClientId);
        //Debug.Log("\nNew Client : " + clientId);
        
        moveVec = Vector3.zero;
        
    }
}
