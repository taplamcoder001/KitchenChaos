using System;
using UnityEngine;

public abstract class State<EState> where EState : Enum
{
    public EState StateKey { get; private set; }

    public State(EState key)
    {
        StateKey = key;
    }

    public abstract EState GetNextState();
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void OnTriggerEnter(Collider collider);
    public abstract void OnTriggerStay(Collider collider);
    public abstract void OnTriggerExit(Collider collider);
}
