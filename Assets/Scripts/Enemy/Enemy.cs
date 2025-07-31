using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 5f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public float knockbackForce = 5f;

    public bool IsGrounded = true;
    public bool IsDead = false;

    private Transform player;
    private Rigidbody playerRb;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || playerRb == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        Vector3 direction = (player.position - transform.position).normalized;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        animator.SetBool("IsGrounded", IsGrounded);

        if (distance <= attackRange)
        {
            animator.SetBool("IsAttacking", true);

            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else
        {
            animator.SetBool("IsAttacking", false);

            if (distance <= chaseRange)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                if (IsGrounded)
                    animator.Play("Walk");
                else
                    animator.Play("Flying");
            }
            else
            {
                if (IsGrounded)
                    animator.Play("Idle");
                else
                    animator.Play("Flying");
            }
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log("Enemy attacks!");

        Vector3 knockbackDir = (player.position - transform.position).normalized;
        playerRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);

        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
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
