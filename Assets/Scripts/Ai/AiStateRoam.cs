using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateRoam : AiState
{
    public AiStateRoam(AiControllerState cs) : base(cs)
    {
    }
    
    protected override void OnEnter()
    {
        base.OnEnter();
        destination = FindRandomPosition();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Vector3.Distance(cs.PlayerPos, cs.EnemyPos) < 2 * GridManager.Singleton.SizeBetweenRooms)
        {
            cs.ChangeState(cs.chaseState);
        }
        else
        {
            if (cs.EnemyPos == destination)
            {
                destination = FindRandomPosition();
            }
            else
            {
                Debug.Log("Roaming");
            }
                
        }
    }

    private Vector3 FindRandomPosition()
    {
        Room r = null;
        int x = 0;
        int z = 0;
        int playerX = Mathf.RoundToInt(cs.PlayerPos.x / GridManager.Singleton.SizeBetweenRooms);
        int playerZ = Mathf.RoundToInt(cs.PlayerPos.z / GridManager.Singleton.SizeBetweenRooms);
        
        do
        {
            x = Random.Range(playerX - 2 , playerX + 2);
            x = Mathf.Clamp(x, 0, GridManager.Singleton.GridWidth - 1);

            z = Random.Range(playerZ - 2, playerZ + 2);
            z = Mathf.Clamp(z, 0, GridManager.Singleton.GridHeight - 1);

            r = cs.Rooms[x, z];
        } while (r is null);
        
        return r.GetPosition() * GridManager.Singleton.SizeBetweenRooms;
    }
}
