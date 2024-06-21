using UnityEngine;
public class FollowingState : EnemyAIBaseState
{
    private float followingDuration;
    private float elapsedTime;

    public override void EnterState(EnemyAIStateManager enemy)
	{
        Debug.Log("Entered FollowingState!");

        enemy.isFollowing = true;

        if (enemy.navMeshAgent.isStopped)
        {
            enemy.navMeshAgent.isStopped = false;
        }

		enemy.animator.SetBool("isLunaticWalking", true);

        elapsedTime = 0f;
        followingDuration = Random.Range(5f, 20f);
    }

	public override void ExitState(EnemyAIStateManager enemy)
	{
        enemy.isFollowing = false;
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
        enemy.Following();

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= followingDuration)
        {
            if (Random.value > 0.6f)
            {
                enemy.SwitchState(enemy.idleState);
            }
            else
            {
                enemy.SwitchState(enemy.patrollingState);
            }
        }
	}
}