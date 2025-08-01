using UnityEngine;
using UnityEngine.UI;

public class HealthAndFuelUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider fuelBar;

    private Health playerHealth;
    private Fuel playerFuel;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        playerHealth = playerController.GetComponent<Health>();
        playerFuel = playerController.GetComponent<Fuel>();

        if (playerHealth != null)
            healthBar.maxValue = playerHealth.GetMaxHealth();

        if (playerFuel != null)
            fuelBar.maxValue = playerFuel.GetMaxFuel();
    }

    void Update()
    {
        if (playerHealth != null)
            healthBar.value = playerHealth.GetHealth();

        if (playerFuel != null)
            fuelBar.value = playerFuel.GetFuel();
    }
}
