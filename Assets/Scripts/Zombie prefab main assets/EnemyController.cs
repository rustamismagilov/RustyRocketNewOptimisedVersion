using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float chaseRange = 10f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float attackRange = 2f;

    NavMeshAgent navMeshAgent;
    Animator animator;
    Transform target;

    float distanceToTarget = Mathf.Infinity;
    EnemyHealth enemyHealth;
    bool isProvoked = false;
    bool isDead = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        target = FindFirstObjectByType<Health>().transform;
    }

    void Update()
    {
        if (enemyHealth.IsDead() || isDead)
        {
            navMeshAgent.enabled = false;
            animator.enabled = false;
            enabled = false;
            return;
        }

        distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (isProvoked)
        {
            EngageTarget();
        }
        else if (distanceToTarget <= chaseRange)
        {
            isProvoked = true;
        }

        UpdateAnimation();
    }

    void EngageTarget()
    {
        FaceTarget();

        if (distanceToTarget > attackRange)
        {
            ChaseTarget();
        }
        else
        {
            AttackTarget();
        }
    }

    void ChaseTarget()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);
        animator.SetBool("isAttacking", false);
    }

    void AttackTarget()
    {
        navMeshAgent.isStopped = true;
        animator.SetBool("isAttacking", true);
    }

    void UpdateAnimation()
    {
        float speedPercent = navMeshAgent.velocity.magnitude / navMeshAgent.speed;
        animator.SetFloat("Speed", speedPercent);
    }

    public void TakeHit()
    {
        animator.SetTrigger("Hit");
        isProvoked = true;
    }

    public void Die()
    {
        isDead = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("Die");
    }

    public void OnDamageTaken()
    {
        isProvoked = true;
    }

    public void EndAttack()
    {
        animator.SetBool("isAttacking", false);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
