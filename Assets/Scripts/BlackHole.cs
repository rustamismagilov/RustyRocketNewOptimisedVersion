using UnityEngine;
using TMPro;

public class BlackHole: MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Burger"))
        {
            Destroy(other.gameObject);

            if (messageText != null)
            {
                messageText.text = "Yum! Give me more";
            }

            Debug.Log("Burger eaten");
        }
    }
}
