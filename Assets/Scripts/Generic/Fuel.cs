using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] private int maxFuel = 100;
    private int currentFuel;

    public int GetFuel() => currentFuel;
    public int GetMaxFuel() => maxFuel;

    void Start()
    {
        currentFuel = maxFuel;
    }

    public void AddFuel(int amount)
    {
        currentFuel += amount;
        if (currentFuel > maxFuel)
            currentFuel = maxFuel;

        Debug.Log("Fuel: " + currentFuel);
    }

    public void ConsumeFuel(float amount)
    {
        currentFuel -= Mathf.CeilToInt(amount);
        if (currentFuel < 0) currentFuel = 0;
    }
}
