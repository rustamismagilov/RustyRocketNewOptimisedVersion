using UnityEngine;

public class KeyHintSound: MonoBehaviour
{
    public AudioClip hintSound;
    private bool hasPlayed = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            if (hintSound != null)
            {
                AudioSource.PlayClipAtPoint(hintSound, transform.position);
                hasPlayed = true;
            }
        }
    }
}