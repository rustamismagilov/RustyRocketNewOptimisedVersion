using UnityEngine;

public class Shooter: MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    public float shootInterval = 1f; 
    public float shootTimer;

    public int damage = 10;
    public int alienDamage = 5;

    public bool isAlien = false;
    

    void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            if (isAlien)
            {
                Shoot(alienDamage);
                shootTimer = shootInterval;
            }
            else
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.V))
                {
                    Shoot(damage);
                    shootTimer = shootInterval;
                }
            }
        }
    }


    public void Shoot()
    {
        if (isAlien)
            Shoot(alienDamage);
        else
            Shoot(damage);
    }
    public void Shoot(int damageToDeal)
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(damageToDeal);
            }
        }
    }

    public float GetShootInterval()
    {
        return shootInterval;
    }
}

