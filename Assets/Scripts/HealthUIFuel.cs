using UnityEngine;
using UnityEngine.UI;

public class HealthUIFuel : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider fuelBar;

    void Start()
    {
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