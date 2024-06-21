using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Transform targetEnemy;

    public bool isPlayerAttackedToThisEnemy = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        animator.SetBool("isIdleToStabbing", false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    continue;
                }

                if (hit.collider.CompareTag("Enemy"))
                {
                    MoveToEnemy(hit.collider.transform);
                    break;
                }

                MoveToPosition(hit.point);
                break;
            }
        }

        ControlAnimations();
    }

    private void MoveToEnemy(Transform enemyTransform)
    {
        Debug.Log("Enemy found: Moving to enemy.");
        navMeshAgent.SetDestination(enemyTransform.position);
        targetEnemy = enemyTransform;
        isPlayerAttackedToThisEnemy = true;

        animator.SetBool("isIdleToStabbing", true);

        DeactivateEnemyComponents();
        ControlAnimations();
    }

    private void MoveToPosition(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
        targetEnemy = null;
        isPlayerAttackedToThisEnemy = false;
        animator.SetBool("isIdleToStabbing", false);
        StartCoroutine(StopStabbingAfterTime(1f));
        ControlAnimations();
    }

    private void DeactivateEnemyComponents()
    {
        if (targetEnemy != null && isPlayerAttackedToThisEnemy)
        {
            EnemyAI enemyAI = targetEnemy.GetComponent<EnemyAI>();
            NavMeshAgent enemyNavMeshAgent = targetEnemy.GetComponent<NavMeshAgent>();

            if (enemyAI != null)
            {
                enemyAI.enabled = false;
                Debug.Log("Enemy AI component deactivated on: " + targetEnemy.name);
            }

            if (enemyNavMeshAgent != null)
            {
                enemyNavMeshAgent.enabled = false;
                Debug.Log("NavMeshAgent component deactivated on: " + targetEnemy.name);
            }
        }
    }

    private void ControlAnimations()
    {
        float speed = navMeshAgent.velocity.magnitude;
        animator.SetFloat("Speed", speed);

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (isPlayerAttackedToThisEnemy)
            {
                StartCoroutine(StopStabbingAfterTime(1f));
            }
        }
    }

    IEnumerator StopStabbingAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("isIdleToStabbing", false);
        isPlayerAttackedToThisEnemy = false;
    }
}