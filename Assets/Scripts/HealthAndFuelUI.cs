using UnityEngine;
using UnityEngine.UI;

public class HealthAndFuelUI : MonoBehaviour
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
            healthBar.maxValue = playerHealth.GetMaxHealth();
            fuelBar.maxValue = playerHealth.GetMaxFuel();
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