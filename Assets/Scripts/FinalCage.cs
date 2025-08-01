using UnityEngine;

public class FinalCage : MonoBehaviour
{
    [Header("Cage Settings")]
    [SerializeField] Animator cageAnimator;
    [SerializeField] AudioClip openSound;
    [SerializeField] float openVolume = 1f;

    private AudioSource audioSource;
    private bool isOpen = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.volume = openVolume;
    }

    public void OpenCage()
    {
        if (isOpen) return;

        isOpen = true;

        if (cageAnimator != null)
            cageAnimator.SetTrigger("Open");

        if (openSound != null)
            audioSource.PlayOneShot(openSound);
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}