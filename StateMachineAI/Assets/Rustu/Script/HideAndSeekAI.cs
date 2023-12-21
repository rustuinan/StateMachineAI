using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    public float chaseSpeed = 6f;
    public float detectionRange = 5f;
    public float patrolWaitTime = 2f;

    private Transform player;
    private NavMeshAgent navMeshAgent;
    private int currentPatrolIndex = 0;
    private float patrolTimer = 0f;

    private enum AIState
    {
        Patrol,
        Chase
    }

    private AIState currentState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetPatrolState();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
        navMeshAgent.speed = patrolSpeed;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);

        if (navMeshAgent.remainingDistance < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0f;
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }

        if (CanSeePlayer())
        {
            SetChaseState();
        }
    }

    void Chase()
    {
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.SetDestination(player.position);

        if (!CanSeePlayer())
        {
            SetPatrolState();
        }
    }

    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.position, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void SetPatrolState()
    {
        currentState = AIState.Patrol;
    }

    void SetChaseState()
    {
        currentState = AIState.Chase;
    }
}
