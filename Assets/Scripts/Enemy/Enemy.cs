using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Chase & Attack Settings")]
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 5f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public float knockbackForce = 5f;

    [Header("Grounded State")]
    public bool IsGrounded = true;
    public bool IsDead = false;

    [Header("Patrol (Flying) Settings")]
    [SerializeField] float patrolRadius = 5f;
    [SerializeField] float patrolInterval = 3f;
    [SerializeField] float patrolSpeed = 2f;

    private Transform player;
    private Rigidbody playerRb;
    private Animator animator;
    private float lastAttackTime;

    private Vector3 patrolTarget;
    private float lastPatrolTime;
    private bool isPatrolling = false;

    private Vector3 lastPosition;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }

        animator = GetComponent<Animator>();
        lastPatrolTime = -patrolInterval;
        patrolTarget = transform.position;

        lastPosition = transform.position;
    }

    void Update()
    {
        if (player == null || playerRb == null) return;
        if (IsDead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        float currentSpeed = ((transform.position - lastPosition).magnitude) / Time.deltaTime;
        animator.SetFloat("Speed", currentSpeed);

        animator.SetBool("IsGrounded", IsGrounded);
        animator.SetBool("IsDead", IsDead);

        if (distance <= attackRange)
        {
            animator.SetBool("IsAttacking", true);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                animator.SetTrigger("AttackTrigger");
                Attack();
                lastAttackTime = Time.time;
            }
            animator.SetFloat("Speed", 0f);
            animator.speed = 1f;
        }
        else
        {
            animator.SetBool("IsAttacking", false);

            if (distance <= chaseRange)
            {
                MoveTowards(player.position, moveSpeed);
                RotateTowards(player.position);
            }
            else
            {
                Patrol();
                RotateTowards(patrolTarget);
            }
            animator.speed = IsGrounded ? 1f : 0.3f;
        }

        lastPosition = transform.position;
    }

    void Attack()
    {
        Debug.Log("Enemy attacks!");

        Vector3 knockbackDir = (player.position - transform.position).normalized;
        playerRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    void Patrol()
    {
        if (!isPatrolling || Time.time - lastPatrolTime > patrolInterval || Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
            randomOffset.y = Random.Range(-1f, 3f);
            patrolTarget = transform.position + randomOffset;

            lastPatrolTime = Time.time;
            isPatrolling = true;
        }

        MoveTowards(patrolTarget, patrolSpeed);
    }

    void MoveTowards(Vector3 target, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 5f * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}
