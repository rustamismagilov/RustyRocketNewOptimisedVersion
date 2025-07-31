using UnityEngine;
using UnityEngine.UI;

public class HealthUIFuel : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider fuelBar;

    private Health playerHealth;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        playerHealth = playerController.GetComponent<Health>();

        if (playerHealth != null)
        {
            healthBar.maxValue = playerHealth.GetHealth();
            fuelBar.maxValue = playerHealth.GetFuel();
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            healthBar.value = playerHealth.GetHealth();
            fuelBar.value = playerHealth.GetFuel();
        }
    }
}