using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMotion : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float speed;
    public float Speed => speed;
    private Vector3 pointMovement;
    public Vector3 PointMovement => pointMovement;
    private Transform characterTransform;
    public Transform CharacterTransform => characterTransform;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        characterTransform = transform;
    }

    public void SetUpStats(Vector3 point)
    {
        agent.speed = speed;
        pointMovement = point;
    }

    public void Movement(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void AgentClear()
    {
        agent.ResetPath();
    }
}
