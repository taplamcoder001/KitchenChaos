using UnityEngine;

public class BotWalking : BotState
{
    public BotWalking(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    private Vector3 pointMovement;

    public override BotStateMachine.EBotState GetNextState()
    {
        if(CaculateDistance() < 0.1f)
        {
            return BotStateMachine.EBotState.Siting;
        }
        return StateKey;
    }

    private float CaculateDistance()
    {
        Vector3 characterPos = context.CharacterMotion.CharacterTransform.position;
        Vector3 characterPosNew = new Vector3(characterPos.x,0f,characterPos.z);

        float distance = Vector3.Distance(characterPosNew, new Vector3(pointMovement.x,0f,pointMovement.z));
        return distance;
    }

    public override void EnterState()
    {   
        pointMovement = context.CharacterMotion.PointMovement; // Add pos movement
        context.CharacterMotion.Movement(pointMovement);
    }
}
