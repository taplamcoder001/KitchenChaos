using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BotMotion : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    public void Movement(Vector3 point)
    {
        agent.SetDestination(point);
    }
}
