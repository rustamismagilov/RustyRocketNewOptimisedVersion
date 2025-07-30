using UnityEngine;
using TMPro;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private TextMeshPro messageText;

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Burger":
                Destroy(other.gameObject);

                if (messageText != null)
                {
                    messageText.text = "Yum! Give me more";
                }

                Debug.Log("Burger eaten by black hole");
                break;

            case "Explosive":
                Destroy(other.gameObject);

                if (messageText != null)
                {
                    messageText.text = "Don't throw explosives in here!";
                }

                Debug.Log("Explosives");
                break;
            
            case "Trash":
                Destroy(other.gameObject);

                if (messageText != null)
                {
                    messageText.text = "Yuck!";
                }

                Debug.Log("Trash");
                break;
            
            default:
                //Debug.Log($"Unknown object entered black hole: {other.name}");
                break;
        }
    }
}
