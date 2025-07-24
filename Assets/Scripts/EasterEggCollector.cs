using UnityEngine;

public class EasterEggCollector : MonoBehaviour
{
    public int specialPoints = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EasterEgg"))
        {
            ScoreManager.Instance.AddPoints(specialPoints);
        }
    }
}
