using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitcher: MonoBehaviour
{
    [SerializeField] CinemachineCamera cameraA;
    [SerializeField] CinemachineCamera cameraB;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + " entered " + gameObject.name);
                cameraA.gameObject.SetActive(false);
                cameraB.gameObject.SetActive(true);
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            Debug.Log(other.name + " exited " + gameObject.name);
            cameraA.gameObject.SetActive(true);
            cameraB.gameObject.SetActive(false);
        }
    }

}
