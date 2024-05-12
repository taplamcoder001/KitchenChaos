using UnityEngine;

public class BotSitting : BotState
{
    public BotSitting(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    public override BotStateMachine.EBotState GetNextState()
    {
        return StateKey;
    }

    public override void EnterState()
    {
        Debug.Log("Bot sitting");
        // animation sitting
        // Order dinner
    }

    public override void ExitState()
    {
        
    }


    public override void UpdateState()
    {
        
    }
}
