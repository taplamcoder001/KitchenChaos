using UnityEngine;

public abstract class BotState : State<BotStateMachine.EBotState>
{
    protected BotContext context;

    public BotState(BotContext context, BotStateMachine.EBotState stateKey) : base(stateKey)
    {
        this.context = context;
    }
}
