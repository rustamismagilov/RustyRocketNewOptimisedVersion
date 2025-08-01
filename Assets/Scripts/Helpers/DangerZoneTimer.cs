using System.Collections;
using TMPro;
using UnityEngine;

public class DangerZoneManager : MonoBehaviour
{
    [Header("Zone References")]
    [SerializeField] private Collider warningZone;
    [SerializeField] private Collider dangerZone;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI warningText;

    [Header("Explosion")]
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private AudioClip countdownBeep;
    [SerializeField] private float beepVolume = 0.3f;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 1.5f;
    [SerializeField] private int secondsToReturn = 5;

    [Header("Messages")]
    [SerializeField] private string warningMessage = "Return to safe zone immediately!!!";
    [SerializeField] private string dangerMessage = "You will be neutralized.";

    private Coroutine dangerTimerCoroutine;
    private GameObject player;
    private MeshRenderer[] playerRenderers;
    private bool playerInWarning = false;
    private bool playerInDanger = false;

    private void OnTriggerEnter(Collider other)
    {
        // No direct triggers on this object
    }

    private void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        // Check if player is inside zones
        bool nowInWarning = warningZone.bounds.Contains(player.transform.position);
        bool nowInDanger = dangerZone.bounds.Contains(player.transform.position);

        // Leaving warning zone but still in danger
        if (!nowInWarning && nowInDanger && playerInWarning)
        {
            ShowWarningOnly();
        }

        // Leaving danger zone completely
        if (!nowInDanger && !nowInWarning && (playerInDanger || playerInWarning))
        {
            StartDangerTimer();
        }

        // Re-entering danger zone (from outside)
        if (nowInDanger && !playerInDanger)
        {
            CancelDangerTimer();
        }

        // Re-entering warning zone (from outside)
        if (nowInWarning && !playerInWarning)
        {
            CancelDangerTimer();
            ShowMessage(""); // clear messages
        }

        playerInWarning = nowInWarning;
        playerInDanger = nowInDanger;
    }

    private void ShowWarningOnly()
    {
        CancelDangerTimer();
        ShowMessage(warningMessage);
    }

    private void StartDangerTimer()
    {
        if (dangerTimerCoroutine != null) return;
        ShowMessage(dangerMessage);
        timerText.enabled = true;
        dangerTimerCoroutine = StartCoroutine(DangerCountdown());
    }

    private void CancelDangerTimer()
    {
        if (dangerTimerCoroutine != null)
        {
            StopCoroutine(dangerTimerCoroutine);
            dangerTimerCoroutine = null;
        }
        ResetUI();
    }

    private void ShowMessage(string message)
    {
        warningText.enabled = true;
        warningText.text = message;
    }

    private IEnumerator DangerCountdown()
    {
        int seconds = secondsToReturn;

        while (seconds > 0)
        {
            timerText.text = seconds.ToString();

            if (countdownBeep != null)
                AudioSource.PlayClipAtPoint(countdownBeep, player.transform.position, beepVolume);

            yield return new WaitForSeconds(1f);
            seconds--;
        }

        // Explosion + Hide player meshes
        if (player != null)
        {
            if (playerRenderers == null)
                playerRenderers = player.GetComponentsInChildren<MeshRenderer>();

            if (explosionEffect != null)
            {
                var fx = Instantiate(explosionEffect, player.transform.position, Quaternion.identity);
                Destroy(fx.gameObject, fx.main.duration + fx.main.startLifetime.constantMax);
            }

            foreach (var rend in playerRenderers)
                rend.enabled = false;

            yield return new WaitForSeconds(respawnDelay);

            // Respawn
            player.transform.SetPositionAndRotation(respawnPoint.position, Quaternion.identity);
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

            foreach (var rend in playerRenderers)
                rend.enabled = true;
        }

        ResetUI();
        dangerTimerCoroutine = null;
    }

    private void ResetUI()
    {
        timerText.text = "";
        timerText.enabled = false;
        warningText.enabled = false;
    }
}
