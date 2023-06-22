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
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (IsOwner) return;
        //moveVec = (playerTransform.position - transform.position).normalized;

        //if (!IsClient) return;
        rb.velocity = moveVec * moveSpeed * Time.fixedDeltaTime;
        Position.Value = transform.position;
    }

    public void OnMoveGhost(InputValue input)
    {
        if (IsOwner) return;
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
}
