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
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Vector3.Distance(cs.PlayerPos, cs.EnemyPos) < 10f)
        {
            cs.ChangeState(cs.roamState);
        }


    }
}
