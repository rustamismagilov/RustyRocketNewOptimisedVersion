using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class DangerZoneTimer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] AudioClip countdownBeep;
    [SerializeField] float beepVolume = 0.3f;
    [SerializeField] Transform respawnPoint; 
    private Coroutine dangerTime;
    private bool exploded = false;

    private void Start()
    {
        timerText.enabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        timerText.enabled = true;

        if (other.CompareTag("Player") && dangerTime == null)
        {
            dangerTime = StartCoroutine(DangerCountdown());
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

    IEnumerator DangerCountdown()
    {
        int seconds = 5;
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
            player.transform.position = respawnPoint.transform.position;
            player.transform.rotation = Quaternion.identity;
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
            ParticleSystem explosion = Instantiate(explosionEffect, player.transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, explosion.main.duration + explosion.main.startLifetime.constantMax);
        }
    }

    void ResetTimerUI()
    {
        if (timerText != null)
            timerText.text = "";
    }
}
