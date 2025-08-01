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

    private Coroutine dangerTime;
    private bool exploded = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dangerTime == null)
        {
            dangerTime = StartCoroutine(DangerCountdown());
        }
    }

    void OnTriggerExit(Collider other)
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

        GameObject start = GameObject.FindWithTag("Start");
        if (start != null && player != null)
        {
            player.transform.position = start.transform.position;
            player.transform.rotation = start.transform.rotation;
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
