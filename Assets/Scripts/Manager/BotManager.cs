using UnityEngine;

public class BotManager : Singleton<BotManager>
{
    [SerializeField] private Transform pointInactiveBot;
    public Transform PointInactiveBot => pointInactiveBot;
}