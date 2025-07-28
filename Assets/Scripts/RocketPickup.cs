using UnityEngine;

public class RocketPickup : MonoBehaviour
{
    public Transform holdPoint;
    private bool isHeld = false;

    void OnMouseDown()
    {
        if (!isHeld)
        {
            // Pick up
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
            isHeld = true;
        }
        else
        {
            // Drop
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = false;
            transform.SetParent(null);
            isHeld = false;
        }
    }
}