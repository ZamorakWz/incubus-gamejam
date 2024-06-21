using UnityEngine;

public class IdleState : EnemyAIBaseState
{
	private float idleDuration;
	private float elapsedTime;

	public override void EnterState(EnemyAIStateManager enemy)
	{
		Debug.Log("Entered IdleState!");

        elapsedTime = 0f;
        idleDuration = Random.Range(3f, 12f);

		enemy.navMeshAgent.isStopped = true;
	}

	public override void ExitState(EnemyAIStateManager enemy)
	{

	}

    public override void OnTriggerEnter(EnemyAIStateManager enemy, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.SwitchState(enemy.followingState);
        }
    }

    public override void OnTriggerExit(EnemyAIStateManager enemy, Collider other)
    {
    }

    public override void OnTriggerStay(EnemyAIStateManager enemy, Collider other)
    {
    }

    public override void UpdateState(EnemyAIStateManager enemy)
	{
		elapsedTime += Time.deltaTime;

		if (elapsedTime >= idleDuration)
		{
			Debug.Log("The state is changed as Patrolling from Idle!");

			enemy.SwitchState(enemy.patrollingState);
		}
	}
}