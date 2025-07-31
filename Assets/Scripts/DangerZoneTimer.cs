using UnityEngine;
using System.Collections;
using TMPro;

public class DangerZoneTimer: MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private ParticleSystem explosionEffect;
    
    private Coroutine dangerTime;
    private bool exploded = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CompareTag("DangerZone"))
        {
            if (dangerTime == null)
            {
                Debug.Log("5 seconds to return!!!");
                dangerTime = StartCoroutine(DangerCountdown());
            }
        }
        else if (other.CompareTag("Player") && CompareTag("SafeZone"))
        {
            if (dangerTime != null)
            {
                Debug.Log("Safe zone entered yay");
                StopCoroutine(dangerTime);
                dangerTime = null;
                ResetTimerUI();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && CompareTag("DangerZone"))
        {
            if (dangerTime != null)
            {
                StopCoroutine(dangerTime);
                dangerTime = null;
                ResetTimerUI();
            }
        }
    }

    IEnumerator DangerCountdown()
    {
        int seconds = 5;
        exploded = false;

        while (seconds > 0)
        {
            Debug.Log(seconds + " seconds left.");
            if (timerText != null)
                timerText.text = seconds.ToString();

            yield return new WaitForSeconds(1f);
            seconds--;
        }

        if (!exploded)
        {
            exploded = true;
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, player.transform.position, Quaternion.identity);
            }
        }

        GameObject start = GameObject.FindWithTag("Start");

        if (start != null)
        {
            player.transform.position = start.transform.position;
            ResetTimerUI();
        }
        

    dangerTime = null;
    }

void ResetTimerUI()
{
    if (timerText != null)
        timerText.text = "5";
}
}
