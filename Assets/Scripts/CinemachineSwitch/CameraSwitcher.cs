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
                cameraB.gameObject.SetActive(false);
                cameraA.gameObject.SetActive(true);
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            Debug.Log(other.name + " exited " + gameObject.name);
            cameraB.gameObject.SetActive(true);
            cameraA.gameObject.SetActive(false);
        }
    }

}
