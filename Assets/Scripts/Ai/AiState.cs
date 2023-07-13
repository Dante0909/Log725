using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState
{
    protected AiControllerState cs;
    
    protected Vector3 destination;
    public Vector3 Destination => destination;
    public AiState(AiControllerState cs)
    {
        this.cs = cs;
    }

    protected virtual void OnEnter()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    public void Exit()
    {
        OnExit();
    }

    public void Enter()
    {
        OnEnter();
    }

    public void Update()
    {
        OnUpdate();
    }
    protected virtual void OnExit()
    {

    }

    
}
