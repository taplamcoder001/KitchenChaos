using System;
using UnityEngine;

// Sharing data between states
public class BotContext
{
    private CharacterMotion characterMotion;
    private BotAnimation botAnimation;
    private BotInteract botInteract;

    // Contructor
    public BotContext(CharacterMotion motion,BotInteract botInteract, BotAnimation animation)
    {
        characterMotion = motion;
        botAnimation = animation;
        this.botInteract = botInteract;
    }

    public CharacterMotion CharacterMotion => characterMotion;
    public BotInteract BotInteract => botInteract;
    public BotAnimation BotAnimation => botAnimation;
}
