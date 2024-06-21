using UnityEngine;
using UnityEngine.AI;

public class EnemyAIEnum : MonoBehaviour
{
	public enum State
	{
		Idle,
		Patrolling,
		Chasing,
		Attack
	}

	public State currentState = State.Idle;

	public Transform player;
	public Transform[] patrolPoints;
	private int currentPatrolPoint = 0;

	private NavMeshAgent agent;

	public float idleProbability = 0.5f;
	public float raycastLength = 5f;

	private Animator animator;
	private float attackProbability = 0.5f;

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		InvokeRepeating("RandomizeState", 0f, 5f);
	}

	void Update()
	{
		switch (currentState)
		{
			case State.Idle:
				Idle();
				break;
			case State.Patrolling:
				Patrolling();
				break;
			case State.Chasing:
				Chasing();
				break;
		}

		PerformRaycastCheck();
		CheckDistanceToPlayer();
	}

	private void Idle()
	{
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
		{
			agent.isStopped = true;
			ChangeState(State.Patrolling);
			animator.SetBool("isLunaticWalking", false);
		}
	}

	private void Patrolling()
	{
		agent.isStopped = false;
		if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
		{
			agent.destination = patrolPoints[currentPatrolPoint].position;
			currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
			animator.SetBool("isLunaticWalking", true);
		}
	}

	private void Chasing()
	{
		agent.isStopped = false;
		agent.destination = player.position;
		animator.SetBool("isLunaticWalking", true);
	}

	private void DoAttack()
	{
		animator.SetBool("isLunaticAttacking", true);
	}

	private void StopAttack()
	{
		animator.SetBool("isLunaticAttacking", false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (Random.value < attackProbability)
			{
				DoAttack();
			}
			else
			{
				StopAttack();
			}
		}
		else
		{
			StopAttack();
		}
	}

	private void ChangeState(State newState)
	{
		currentState = newState;
	}

	private void RandomizeState()
	{
		if (currentState == State.Chasing || currentState == State.Attack)
			return;

		float randomValue = Random.value;
		if (randomValue < idleProbability)
		{
			ChangeState(State.Idle);
			animator.SetBool("isLunaticWalking", false);
		}
		else
		{
			ChangeState(State.Patrolling);
			animator.SetBool("isLunaticWalking", true);
		}
	}

	private void PerformRaycastCheck()
	{
		RaycastHit hit;
		Vector3 direction = player.position - transform.position;

		if (Physics.Raycast(transform.position, direction.normalized, out hit, raycastLength))
		{
			if (hit.transform.CompareTag("Player"))
			{
				ChangeState(State.Chasing);
				animator.SetBool("isLunaticWalking", true);
				Debug.Log("Player detected - switching to Chasing state");
			}
		}
	}

	private void CheckDistanceToPlayer()
	{
		float distance = Vector3.Distance(player.position, transform.position);
		if (distance > 10 && (currentState == State.Chasing || currentState == State.Attack))
		{
			RandomizeIdleOrPatrolling();
			Debug.Log("state is randomized");
		}
	}

	private void RandomizeIdleOrPatrolling()
	{
		float randomValue = Random.value;
		if (randomValue < 0.5)
		{
			ChangeState(State.Idle);
			animator.SetBool("isLunaticWalking", false);
		}
		else
		{
			ChangeState(State.Patrolling);
			animator.SetBool("isLunaticWalking", true);
		}
	}
}