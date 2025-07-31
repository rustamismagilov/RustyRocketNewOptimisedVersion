using UnityEngine;

public class HealthRestorer : MonoBehaviour
{
    [SerializeField] private float holdTimeToEat = 1.5f;
    [SerializeField] private int healthToRestore = 20;
    [SerializeField] private int fuelToRestore = 10;

    private float eatTimer = 0f;
    private bool isEating = false;

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            return;
        }

        Pickup pickup = player.GetComponent<Pickup>();
        Health playerHealth = player.GetComponent<Health>();

        if (pickup == null || playerHealth == null)
        {
            return;
        }

        if (pickup.GetHeldPickup() == gameObject)
        {
            if (Input.GetKey(KeyCode.E))
            {
                eatTimer += Time.deltaTime;

                if (eatTimer >= holdTimeToEat && !isEating)
                {
                    isEating = true;
                    playerHealth.RestoreHealth(healthToRestore);
                    playerHealth.AddFuel(fuelToRestore);

                    pickup.RemoveHeldPickup();
                    Destroy(gameObject);

                    Debug.Log($"{gameObject.name} consumed!");
                }
            }
            else
            {
                eatTimer = 0f;
                isEating = false;
            }
        }
        else
        {
            eatTimer = 0f;
            isEating = false;
        }
    }
}
