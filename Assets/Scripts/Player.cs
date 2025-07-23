using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] float backgroundVolume = 1f;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    public Rigidbody rb;
    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;

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

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        float currentThrust = mainThrust;
        bool isThrusting = false;

        // Speed modifiers
        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            currentThrust *= 0.4f;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
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
