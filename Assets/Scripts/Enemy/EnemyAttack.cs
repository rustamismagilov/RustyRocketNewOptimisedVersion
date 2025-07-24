using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Health target;
    [SerializeField] float damage = 24.9f;
    [SerializeField] float attackRange = 2f;

    void Start()
    {
        target = FindFirstObjectByType<Health>();
    }

    public void AttackHitEvent()
    {
        if (target == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToPlayer <= attackRange)
        {
            target.TakeDamage((int)damage);
            Debug.Log("ouch oh no no");
        }
        else
        {
            Debug.Log("Zombie attack missed — player too far away.");
        }
    }
}