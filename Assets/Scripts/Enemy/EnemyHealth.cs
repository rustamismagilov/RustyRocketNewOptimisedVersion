using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float hitPoints = 100f;
    [SerializeField] GameObject bloodEffect;

    bool isDead = false;

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        BroadcastMessage("OnDamageTaken");

        hitPoints -= damage;
        
        if (bloodEffect != null)
        {
            Instantiate(bloodEffect, transform.position + Vector3.up * 1.2f, Quaternion.identity);
        }

        if (hitPoints <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        GetComponent<Animator>().SetTrigger("Die");
    }
}