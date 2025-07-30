using UnityEngine;

public class HealthRestorer : MonoBehaviour
{
    private PickupTest pickupTest;
    private Health playerHealth;

    private void Start()
    {
        pickupTest = GetComponent<PickupTest>();
        playerHealth = GetComponent<Health>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject heldHealthRestoringObject = pickupTest.GetHeldPickup();

            if (heldHealthRestoringObject != null && heldHealthRestoringObject.CompareTag("Burger"))
            {
                playerHealth.RestoreHealth(20);
                playerHealth.AddFuel(10);

                Destroy(heldHealthRestoringObject);
                pickupTest.RemoveHeldPickup();

                Debug.Log($"{name} eaten!");
            }
            else
            {
                Debug.Log("Not edible");
            }
        }
    }
}
