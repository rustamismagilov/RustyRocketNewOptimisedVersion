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
        if (player == null) return;

        CategoryItemSwitcher categorySwitcher = player.GetComponentInChildren<CategoryItemSwitcher>();
        Health playerHealth = player.GetComponent<Health>();

        if (categorySwitcher == null || playerHealth == null) return;

        // Check if this item is currently the active/held item
        GameObject currentItem = categorySwitcher.GetCurrentlyHeldItem();

        if (currentItem == gameObject)
        {
            if (Input.GetKey(KeyCode.E))
            {
                eatTimer += Time.deltaTime;

                if (eatTimer >= holdTimeToEat && !isEating)
                {
                    isEating = true;
                    playerHealth.RestoreHealth(healthToRestore);
                    playerHealth.AddFuel(fuelToRestore);

                    // Remove this item from the category system
                    categorySwitcher.RemoveItem(gameObject);

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
