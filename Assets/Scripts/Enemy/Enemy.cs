using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;
    private bool isStopped = false;
    private float stopUntilTime;

    void Update()
    {
        if (isStopped && Time.time < stopUntilTime)
            return;

        if (isStopped && Time.time >= stopUntilTime)
            isStopped = false;

       //attack
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void StopForSeconds(float duration)
    {
        if (!isStopped)
        {
            isStopped = true;
            stopUntilTime = Time.time + duration;
        }
    }
}