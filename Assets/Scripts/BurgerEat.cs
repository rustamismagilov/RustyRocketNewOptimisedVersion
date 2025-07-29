using UnityEngine;

public class BurgerEater : MonoBehaviour
{
    public PickupTest pickupTest;
    public Health playerHealth;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject heldBurger = pickupTest.GetHeldPickup();

            if (heldBurger != null && heldBurger.CompareTag("Burger"))
            {
                playerHealth.RestoreHealth(20);
                playerHealth.AddFuel(10);

                Destroy(heldBurger);
                pickupTest.RemoveHeldPickup();

                Debug.Log("Burger eaten!");
            }
            else
            {
                Debug.Log("Not edible");
            }

        }
    }

}
