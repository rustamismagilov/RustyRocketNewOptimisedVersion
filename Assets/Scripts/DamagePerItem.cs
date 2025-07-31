using UnityEngine;

public class DamagePerItem: MonoBehaviour
{
    [SerializeField] float damageAmount = 5f;
    [SerializeField] ParticleSystem hitEffect;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageAmount);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}