using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    public enum PatrolMode { Fixed, Random }
    public PatrolMode patrolMode = PatrolMode.Fixed;

    [Header("Common Settings")]
    public float waitTimeAtWaypoint = 2f;
    public float stoppingDistance = 1f;

    [Header("Player Detection")]
    public float detectionRadius = 5f;
    public float rotationSpeed = 5f;

    [Header("Fixed Path")]
    public Transform[] patrolPoints;

    [Header("Random Patrol Area")]
    public Vector3 centerPoint;
    public float patrolRadius = 10f;

    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private Animator animator;
    private Transform player;
    private bool playerDetected = false;
    private Vector3 originalForward;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        originalForward = transform.forward;

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
        CheckForPlayer();

        if (playerDetected)
        {
            HandlePlayerDetection();
        }
        else
        {
            HandlePatrol();
        }

        UpdateAnimator();
    }

    void CheckForPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        playerDetected = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                player = hitCollider.transform;
                playerDetected = true;
                break;
            }
        }
    }

    void HandlePlayerDetection()
    {
        agent.isStopped = true;

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void HandlePatrol()
    {
        agent.isStopped = false;

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
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            bool isMoving = agent.velocity.magnitude > 0.1f && !playerDetected;
            animator.SetBool("isMoving", isMoving);
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0)
            return;

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
}