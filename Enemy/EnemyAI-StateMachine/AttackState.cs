using UnityEngine;

public class AttackState : EnemyAIBaseState
{
	public override void EnterState(EnemyAIStateManager enemy)
	{
        Debug.Log("Entered AttackingState!");

        if (enemy.navMeshAgent.isStopped)
        {
            enemy.navMeshAgent.isStopped = false;
        }
    }

    public override void ExitState(EnemyAIStateManager enemy)
	{
	}

    public override void OnTriggerEnter(EnemyAIStateManager enemy, Collider other)
    {

    }

    public override void OnTriggerExit(EnemyAIStateManager enemy, Collider other)
    {

    }

    public override void OnTriggerStay(EnemyAIStateManager enemy, Collider other)
    {

    }

    public override void UpdateState(EnemyAIStateManager enemy)
	{
	}
}