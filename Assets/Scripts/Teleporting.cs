using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Teleporting: MonoBehaviour
{
    [SerializeField] Transform teleportTarget;
    [SerializeField] GameObject player;

    void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportTarget.position;
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
