using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiControllerState : MonoBehaviour
{
    private Transform player;
    public Vector3 EnemyPos => this.transform.position;
    public Vector3 PlayerPos => player.position;

    public AiState currentState;

    public AiStateRoam roamState;

    public AiStateChase chaseState;

    public Grid grid;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        roamState = new AiStateRoam(this);
        chaseState = new AiStateChase(this);
        ChangeState(roamState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update();
    }

    public void ChangeState(AiState newState)
    {
        if (newState is not null && newState != currentState)
        {
            currentState.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public Vector3 GetMoveVec()
    {
        return Vector3.zero;
    }
}
