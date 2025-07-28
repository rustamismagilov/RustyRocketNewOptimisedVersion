using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher: MonoBehaviour
{
    public CinemachineCamera cameraA;
    public CinemachineCamera cameraB;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.CompareTag ("AreaB"))
        {
            {
                cameraA.enabled = false;
                cameraB.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.CompareTag ("AreaB"))
        { 
                cameraB.enabled = false; 
                cameraA.enabled = true;
        }
    }

}
