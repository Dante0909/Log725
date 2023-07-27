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

    private Room[,] rooms;
    public Room[,] Rooms => rooms;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rooms = GridManager.Singleton.GetGrid().GetRooms();
        
        roamState = new AiStateRoam(this);
        chaseState = new AiStateChase(this);
        ChangeState(roamState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(AiState newState)
    {
        if (newState is not null && newState != currentState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }
    }

    public Vector3 GetMoveVec()
    {
        if(currentState is null)
            return Vector3.zero;
        return (currentState.Destination - EnemyPos).normalized;
    }
}
