using UnityEngine;

public class PatrollingState : EnemyAIBaseState
{
    private float patrollingDuration;
    private float elapsedTime;

    public override void EnterState(EnemyAIStateManager enemy)
    {
        Debug.Log("Entered PatrollingState!");

        if (enemy.navMeshAgent.isStopped)
        {
            enemy.navMeshAgent.isStopped = false;
        }
        enemy.isPatrolling = true;

        elapsedTime = 0f;
        patrollingDuration = Random.Range(12f, 36f);
    }

    public override void UpdateState(EnemyAIStateManager enemy)
    {
        enemy.Patrolling();

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= patrollingDuration)
        {
            Debug.Log("The state is changed as Idle from patrolling");

            enemy.SwitchState(enemy.idleState);
        }
    }

    public override void ExitState(EnemyAIStateManager enemy)
    {
        enemy.isPatrolling = false;
    }

    public override void OnTriggerEnter(EnemyAIStateManager enemy, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.isPatrolling = false;
            enemy.SwitchState(enemy.followingState);
        }
    }

    public override void OnTriggerStay(EnemyAIStateManager enemy, Collider other)
    {
    }

    public override void OnTriggerExit(EnemyAIStateManager enemy, Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemy.isPatrolling = false;
        }
    }
}