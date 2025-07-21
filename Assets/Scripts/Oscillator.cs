using UnityEngine;

public class Oscillator: MonoBehaviour
{
    [SerializeField] Vector3 movementVector = Vector3.up;
    Vector3 startingPos;
    [SerializeField] float movementSpeed = 10f;
    [SerializeField] private float distance = 50f;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        float newPos = Mathf.PingPong(Time.time * movementSpeed,distance);
        transform.position = startingPos + movementVector.normalized * newPos;
    }
}


