using UnityEngine;

public class StopEnemyPotion : MonoBehaviour
{
    [SerializeField] float stopDuration = 5f;
    [SerializeField] float stopRadius = 10f;

    [SerializeField] Transform player;

    public void UsePotion()
    {
        if (player == null)
        {
            return;
        }

        Enemy[] enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, player.position);

            if (distance <= stopRadius)
            {
                enemy.StopForSeconds(stopDuration);
            }
        }
    }
}
