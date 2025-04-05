using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public enum PatrolMode { Fixed, Random }
    public PatrolMode patrolMode = PatrolMode.Fixed;

    [Header("Common Settings")]
    public float waitTimeAtWaypoint = 2f;
    public float stoppingDistance = 1f;

    [Header("Fixed Path")]
    public Transform[] patrolPoints;

    [Header("Random Patrol Area")]
    public Vector3 centerPoint;
    public float patrolRadius = 10f;

    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (patrolMode == PatrolMode.Random)
        {
            centerPoint = transform.position;
            SetRandomDestination();
        }
        else if (patrolMode == PatrolMode.Fixed && patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtWaypoint)
            {
                waitTimer = 0f;
                if (patrolMode == PatrolMode.Fixed)
                {
                    GoToNextPoint();
                }
                else if (patrolMode == PatrolMode.Random)
                {
                    SetRandomDestination();
                }
            }
        }

        if (animator != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += centerPoint;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (patrolMode == PatrolMode.Random)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(centerPoint, patrolRadius);
        }
    }
}
