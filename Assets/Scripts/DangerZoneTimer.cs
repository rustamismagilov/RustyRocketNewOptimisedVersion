using UnityEngine;
using System.Collections;
using TMPro;

public class DangerZoneTimer : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private ParticleSystem explosionEffect;

    private Coroutine dangerTime;
    private bool exploded = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dangerTime == null)
        {
            Debug.Log("5 seconds to return!");
            dangerTime = StartCoroutine(DangerCountdown());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && dangerTime != null)
        {
            Debug.Log("Exited Danger zone");
            StopCoroutine(dangerTime);
            dangerTime = null;
            ResetTimerUI();
        }
    }

    IEnumerator DangerCountdown()
    {
        int seconds = 5;
        exploded = false;

        while (seconds > 0)
        {
            Debug.Log($"{seconds} seconds left.");
            if (timerText != null)
                timerText.text = seconds.ToString();

            yield return new WaitForSeconds(1f);
            seconds--;
        }

        yield return new WaitForSeconds(0.25f);

        if (!exploded)
        {
            ExplosionMethod();
        }

        GameObject start = GameObject.FindWithTag("Start");
        if (start != null)
        {
            player.transform.position = start.transform.position;
        }

        ResetTimerUI();
        dangerTime = null;
    }

    void ExplosionMethod()
    {
        if (exploded) return;

        exploded = true;

        if (explosionEffect != null)
        {
            Debug.Log("Explosion initiated!");
            Vector3 spawnPos = player.transform.position;
            ParticleSystem explosion = Instantiate(explosionEffect, spawnPos, Quaternion.identity);
            Destroy(explosion.gameObject, explosion.main.duration + explosion.main.startLifetime.constantMax);
        }
    }

    void ResetTimerUI()
    {
        if (timerText != null)
            timerText.text = "";
    }
}
