using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalLoader : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            SceneManager.LoadScene("Planet 2");
        }
    }
}

