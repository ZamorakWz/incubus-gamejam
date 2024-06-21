using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyAIBaseState
{
	public abstract void EnterState(EnemyAIStateManager enemy);
	public abstract void UpdateState(EnemyAIStateManager enemy);
	public abstract void ExitState(EnemyAIStateManager enemy);
	public abstract void OnTriggerEnter(EnemyAIStateManager enemy, Collider other);

	public abstract void OnTriggerStay(EnemyAIStateManager enemy, Collider other);

	public abstract void OnTriggerExit(EnemyAIStateManager enemy, Collider other);
}
