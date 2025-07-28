using UnityEngine;
using System.Collections;

public class DangerZoneTimer: MonoBehaviour
{
    [SerializeField] GameObject player;

    private Coroutine dangerTime;

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
            }
        }
    }

    IEnumerator DangerCountdown()
    { 
        int seconds = 5;
        while (seconds > 0)
        { 
            Debug.Log( seconds + " seconds left.");
            yield return new WaitForSeconds(1f);
            seconds--;
        }
        
        GameObject start = GameObject.FindWithTag("Start");
        
        if (start != null) 
        {
            player.transform.position = start.transform.position;
        }

        dangerTime = null;
    }
}