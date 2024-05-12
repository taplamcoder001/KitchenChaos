using System;
using UnityEngine;

public abstract class State<EState> where EState : Enum
{
    public State(EState key)
    {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    public abstract EState GetNextState();
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void UpdateState() { }
    public virtual void OnTriggerEnter(Collider collider) { }
    public virtual void OnTriggerStay(Collider collider) { }
    public virtual void OnTriggerExit(Collider collider) { }
}
