using UnityEngine;
using System.Collections.Generic;

public class EnemyDetectionTrigger : MonoBehaviour
{
    AlienCompanion alienCompanion;
    List<Transform> enemiesInRange = new List<Transform>();

    private void Start()
    {
        alienCompanion = FindFirstObjectByType<AlienCompanion>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.transform);
            alienCompanion.SetCombatTarget(GetClosestEnemy());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.transform);

            if (enemiesInRange.Count == 0)
                alienCompanion.SetCombatTarget(null);
            else
                alienCompanion.SetCombatTarget(GetClosestEnemy());
        }
    }

    Transform GetClosestEnemy()
    {
        Transform closest = null;
        float minSqr = float.MaxValue;
        Vector3 center = transform.position;

        foreach (var enemy in enemiesInRange)
        {
            float sqr = (enemy.position - center).sqrMagnitude;
            if (sqr < minSqr)
            {
                closest = enemy;
                minSqr = sqr;
            }
        }

        return closest;
    }
}
