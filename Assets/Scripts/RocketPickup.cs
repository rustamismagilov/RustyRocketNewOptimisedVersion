using UnityEngine;

public class RocketPickup: MonoBehaviour
{
    public Transform holdPoint;
    private GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && heldObject == null)
            TryPickup();

        if (Input.GetKeyDown(KeyCode.Q) && heldObject != null)
            Drop();
    }

    void TryPickup()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3f))
        {
            var target = hit.collider.gameObject;
            if (!target.CompareTag("Pickup")) return;

            var potion = target.GetComponent<StopEnemyPotion>();
            if (potion != null)
            {
                potion.UsePotion();
                Destroy(target);
                return;
            }

            heldObject = target;
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;

            var rb = heldObject.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
        }
    }

    void Drop()
    {
        var rb = heldObject.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        heldObject.transform.SetParent(null);
        heldObject = null;
    }
}