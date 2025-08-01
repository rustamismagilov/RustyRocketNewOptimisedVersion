using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Ranges")]
    public float chaseRange = 10f;
    public float attackRange = 2f;

    [Header("Movement & Attack")]
    public float moveSpeed = 5f;
    public float attackCooldown = 1.5f;
    public int damage = 1;
    public float knockbackForce = 5f;

    private Transform player;
    private Rigidbody playerRb;
    
    //private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }

        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || playerRb == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
        
        if (distance <= chaseRange && distance > attackRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        
        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    void Attack()
    {
        //if (animator != null)
            //animator.SetTrigger("Attack");

        Debug.Log("Enemy attacks!");
        
        Vector3 knockbackDir = (player.position - transform.position).normalized;
        playerRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
        
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        //TODO: Take Damage
    }
}
