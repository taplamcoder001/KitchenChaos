using UnityEngine;

public class BotOutRestaurant : BotState
{
    public BotOutRestaurant(BotContext context, BotStateMachine.EBotState stateKey) : base(context, stateKey)
    {
    }

    private Vector3 pointMovement;

    public override BotStateMachine.EBotState GetNextState()
    {
        if(CaculateDistance()<= 0.1f)
        {
            context.BotAnimation.DisActive();
        }
        return StateKey;
    }

    private float CaculateDistance()
    {
        pointMovement = BotManager.Instance.PointInactiveBot.position;
        Vector3 characterPos = context.CharacterMotion.CharacterTransform.position;
        Vector3 characterPosNew = new Vector3(characterPos.x,0f,characterPos.z);

        float distance = Vector3.Distance(characterPosNew, new Vector3(pointMovement.x,0f,pointMovement.z));
        return distance;
    }

    public override void EnterState()
    {
        context.BotInteract.TableScript.ResetTable();
        pointMovement = context.CharacterMotion.PointMovement; 
        context.CharacterMotion.Movement(pointMovement);
    }
}
