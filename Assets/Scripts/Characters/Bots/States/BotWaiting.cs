using UnityEngine;

public class BotWaiting : BotState
{
    public BotWaiting(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    public override BotStateMachine.EBotState GetNextState()
    {
        return StateKey;
    }

    public override void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }
}
