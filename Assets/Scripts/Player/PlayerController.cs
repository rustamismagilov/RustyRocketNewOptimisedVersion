using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Thrust Settings")]
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    private Fuel fuel;
    [SerializeField] private float fuelConsumptionRate = 5f; // per second
    [SerializeField] private float fuelConsumptionBoostedMultiplier = 2f;

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
        fuel = GetComponent<Fuel>();

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
        float targetDamping = Input.GetKey(KeyCode.LeftAlt) ? linearDampingToSlowdown : 0f;
        float dampingChangeRate = linearDampingToSlowdown / linearDampingChangeRate;
        rb.linearDamping = Mathf.MoveTowards(rb.linearDamping, targetDamping, dampingChangeRate * Time.deltaTime);

        bool isBoosting = Input.GetKey(KeyCode.LeftShift);
        if (isBoosting)
            currentThrust *= fuelConsumptionBoostedMultiplier;

        float currentFuelConsumption = fuelConsumptionRate * (isBoosting ? fuelConsumptionBoostedMultiplier : 1f);

        bool isPressingSpace = Input.GetKey(KeyCode.Space);
        bool hasFuel = fuel != null && fuel.GetFuel() > 0;

        if (isPressingSpace && hasFuel)
        {
            rb.AddRelativeForce(Vector3.up * (currentThrust * Time.deltaTime));
            fuel.ConsumeFuel(currentFuelConsumption * Time.deltaTime);
            isThrusting = true;

            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(mainEngineSound);

            if (!mainEngineParticles.isPlaying)
                mainEngineParticles.Play();
        }
        else
        {
            if (mainEngineParticles.isPlaying)
                mainEngineParticles.Stop();

            if (audioSource.isPlaying)
                audioSource.Stop();
        }

        // Forward thrust (does not consume fuel but still applies force)
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddRelativeForce(Vector3.forward * (currentThrust * Time.deltaTime));
            isThrusting = true;
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
