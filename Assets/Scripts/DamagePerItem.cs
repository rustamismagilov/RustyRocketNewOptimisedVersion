using UnityEngine;

public class DamagePerItem: MonoBehaviour
{
    [SerializeField] float damageAmount = 5f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}