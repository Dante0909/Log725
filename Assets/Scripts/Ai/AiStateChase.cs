using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStateChase : AiState
{
    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        destination = (cs.PlayerPos - cs.EnemyPos).normalized;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if(Vector3.Distance(cs.PlayerPos, cs.EnemyPos) > 0.5f * GridManager.Singleton.SizeBetweenRooms)
            cs.ChangeState(cs.roamState);
        else
        {
            destination = cs.PlayerPos;
        }
        
    }

    public AiStateChase(AiControllerState cs) : base(cs)
    {
    }
}
