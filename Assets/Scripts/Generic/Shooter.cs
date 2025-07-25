using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float shootInterval = 1f;
    [SerializeField] float shootTimer;

    [SerializeField] int damage = 10;
    [SerializeField] int alienDamage = 5;

    [SerializeField] bool isAlien = false;

    bool manualShootingEnabled = true;

    public void SetManualShootingEnabled(bool enabled)
    {
        manualShootingEnabled = enabled;
    }

    public int GetDamage()
    {
        return damage;
    }


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
            else if (manualShootingEnabled && Input.GetMouseButton(0))
            {
                Shoot(damage);
                shootTimer = shootInterval;
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

