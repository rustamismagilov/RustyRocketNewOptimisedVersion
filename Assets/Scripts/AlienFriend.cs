using UnityEngine;

public class AlienFriend: MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float followDistance = 10f;

    private Shooter shooter;
    private float shootTimer = 0f;

    void Start()
    {
        shooter = GetComponent<Shooter>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector3 direction = player.position - transform.position;

        if (direction.magnitude > followDistance)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
        }

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f && direction.magnitude <= followDistance)
        {
            shooter.Shoot();
            shootTimer = shooter.GetShootInterval(); 
        }
    }
}
