using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStateManager : MonoBehaviour
{
    // State references
    private EnemyAIBaseState currentState;
    public IdleState idleState = new IdleState();
    public FollowingState followingState = new FollowingState();
    public AttackState attackingState = new AttackState();
    public PatrollingState patrollingState = new PatrollingState();

    // State-bools references
    public bool isFollowing = false;

    // Animation references
    public Animator animator;

    // Player references
    public Transform playerTransform;

    // Collision references
    public Collider[] withinAggroColliders;
    public bool isPatrolling = false;
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    // NavMeshAgent reference
    public NavMeshAgent navMeshAgent;

    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        currentState.UpdateState(this);
        ControlAnimations();
    }

    public void SwitchState(EnemyAIBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);

        Debug.Log("Switched to " + state.GetType().Name);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(this, other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(this, other);
    }

    private void ControlAnimations()
    {
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("LunaticSpeed", speed);
    }

    public void Patrolling()
    {
        if (!isPatrolling || waypoints.Length == 0) return;

        navMeshAgent.destination = waypoints[currentWaypointIndex].position;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    public void Following()
    {
        if (isFollowing)
        {
            if (!navMeshAgent.isStopped)
            {
                if (navMeshAgent.pathPending || Vector3.Distance(navMeshAgent.destination, playerTransform.position) > 2.0f)
                {
                    navMeshAgent.destination = playerTransform.position;
                }
            }
        }
    }
}