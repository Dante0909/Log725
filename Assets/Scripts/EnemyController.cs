using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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
        moveVec = (playerTransform.position - transform.position).normalized;

        transform.Translate(moveVec * moveSpeed * Time.deltaTime);
        Position.Value = transform.position;
    }
}
