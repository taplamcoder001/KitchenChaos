using System.Collections;
using UnityEngine;

public class BotInit : BotState
{
    public BotInit(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    private float timeCurrent;

    public override BotStateMachine.EBotState GetNextState()
    {
        if (timeCurrent > 2)
        {
            return BotStateMachine.EBotState.Walking;
        }
        return StateKey;
    }

    public override void UpdateState()
    {
        timeCurrent += Time.deltaTime;
    }
}
