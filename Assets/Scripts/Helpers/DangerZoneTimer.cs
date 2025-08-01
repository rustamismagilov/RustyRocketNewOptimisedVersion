using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class DangerZoneTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] AudioClip countdownBeep;
    [SerializeField] float beepVolume = 0.3f;
    [SerializeField] Transform respawnPoint;
    [SerializeField] int secondsToReturn = 5;
    [SerializeField] private string warningMessage = "Return to safe zone immediately!!!";
    [SerializeField] private string dangerMessage = "You will be neutralized.";

    private Coroutine dangerTime;
    private bool exploded = false;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        warningText.enabled = false;
        timerText.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        warningText.enabled = true;
        warningText.text = warningMessage.ToString();
        timerText.enabled = true;

        if (other.CompareTag("Player") && dangerTime == null)
        {
            dangerTime = StartCoroutine(Countdown());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dangerTime != null)
        {
            StopCoroutine(dangerTime);
            dangerTime = null;
            exploded = false;
            ResetTimerUI();
        }
    }

    IEnumerator Countdown()
    {
        int seconds = secondsToReturn;
        exploded = false;

        while (seconds > 0)
        {
            if (timerText != null)
                timerText.text = seconds.ToString();

            if (countdownBeep != null)
                AudioSource.PlayClipAtPoint(countdownBeep, player.transform.position, beepVolume);

            yield return new WaitForSeconds(1f);
            seconds--;
        }

        if (!exploded)
        {
            ExplosionMethod();
        }


        if (respawnPoint != null && player != null)
        {
            player.transform.SetPositionAndRotation(respawnPoint.transform.position, Quaternion.identity);
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        ResetTimerUI();
        dangerTime = null;
    }

    void ExplosionMethod()
    {
        if (exploded) return;
        exploded = true;

        if (explosionEffect != null && player != null)
        {
            explosionEffect = Instantiate(explosionEffect, player.transform.position, Quaternion.identity);
            Destroy(explosionEffect.gameObject, explosionEffect.main.duration + explosionEffect.main.startLifetime.constantMax);
        }
    }
    void ResetTimerUI()
    {
        if (timerText != null)
            timerText.text = "";
    }
}
