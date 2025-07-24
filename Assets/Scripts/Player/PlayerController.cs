using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Thrust Settings")]
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] float backgroundVolume = 1f;

    [Header("Particle Effects")]
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    [Header("Damping Settings")]
    [SerializeField] float linearDampingToSlowdown = 10f;
    [SerializeField] float linearDampingChangeRate = 2f;

    Rigidbody rb;
    AudioSource audioSource;
    AudioSource backgroundAudioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = gameObject.AddComponent<AudioSource>();

        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource.clip = backgroundMusic;
        backgroundAudioSource.volume = backgroundVolume;
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        float currentThrust = mainThrust;
        bool isThrusting = false;

        // Speed modifiers
        // If the left Alt key is pressed, apply linear damping
        // to slow down the Player's movement
        float targetDamping = Input.GetKey(KeyCode.LeftAlt) ? linearDampingToSlowdown : 0f;

        // how fast to change damping: total delta (10) over 2 seconds
        float dampingChangeRate = linearDampingToSlowdown / linearDampingChangeRate;

        // move current damping toward target at that rate
        rb.linearDamping = Mathf.MoveTowards(rb.linearDamping, targetDamping, dampingChangeRate * Time.deltaTime);        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentThrust *= 2f;
        }

        // Upward thrust
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * (currentThrust * Time.deltaTime));
            isThrusting = true;

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngineSound);
            }
        }

        // Forward thrust
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.forward * (currentThrust * Time.deltaTime));
            isThrusting = true;
        }

        // Particles and sound
        if (isThrusting)
        {
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void ProcessRotation()
    {
        bool isRotating = false;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(-rotationThrust);
            isRotating = true;

            if (!rightThrusterParticles.isPlaying)
                rightThrusterParticles.Play();
        }
        else
        {
            rightThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(rotationThrust);
            isRotating = true;

            if (!leftThrusterParticles.isPlaying)
                leftThrusterParticles.Play();
        }
        else
        {
            leftThrusterParticles.Stop();
        }

        if (isRotating)
        {
            // Add forward movement while rotating
            rb.AddRelativeForce(Vector3.forward * (mainThrust * 0.5f * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddRelativeForce(-Vector3.forward * (mainThrust * Time.deltaTime));
        }
    }


    void ApplyRotation(float rotationThisFrame)
    {
        float rotationAngle = rotationThisFrame * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0f, rotationAngle, 0f);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
