using UnityEngine;

public class BotSitting : BotState
{
    public BotSitting(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    private float timeEating = 10f;
    private float currentTime;

    public override BotStateMachine.EBotState GetNextState()
    {
        if(currentTime > timeEating)
        {
            return BotStateMachine.EBotState.OutRestaurant;
        }
        return StateKey;
    }

    public override void EnterState()
    {
        currentTime = timeEating;
        context.BotAnimation.GetPosForBody(context.CharacterMotion.PointMovement);
    }

    public override void ExitState()
    {
        context.BotAnimation.GetPosForBody(Vector3.zero);
    }

    public override void UpdateState()
    {
        if(context.BotInteract.TableScript.HasFood)
        {
            currentTime += Time.deltaTime;
        }
    }
}
