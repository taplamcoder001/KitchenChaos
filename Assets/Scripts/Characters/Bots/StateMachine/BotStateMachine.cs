using UnityEngine;

[RequireComponent(typeof(CharacterMotion))]
public class BotStateMachine : StateMachine<BotStateMachine.EBotState>
{
    public enum EBotState
    {
        Init,
        Walking,
        Siting,
        OutRestaurant
    }

    private BotContext context;
    
    private CharacterMotion characterMotion;
    private BotAnimation botAnimator;
    private BotInteract botInteract;

    private void Awake()
    {
        characterMotion = GetComponent<CharacterMotion>();
        botAnimator = GetComponent<BotAnimation>();
        botInteract = GetComponent<BotInteract>();
        
        context = new BotContext(characterMotion, botInteract, botAnimator);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(EBotState.Init, new BotInit(context, EBotState.Init));
        States.Add(EBotState.Walking, new BotWalking(context, EBotState.Walking));
        States.Add(EBotState.Siting, new BotSitting(context, EBotState.Siting));
        States.Add(EBotState.OutRestaurant, new BotSitting(context, EBotState.OutRestaurant));

        CurrentState = States[EBotState.Init];
    }
}
