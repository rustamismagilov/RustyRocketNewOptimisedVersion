using UnityEngine;

public class Player: MonoBehaviour
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
        // audioSource= FindFirstObjectByType<AudioSource>(); //only if there is only one audiosource
        
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

        // Speed
        
        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            currentThrust *= 0.4f; // Safe landing / slow down
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentThrust *= 2f; // Speed boost
        }

        // Upward thrust
        
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce (Vector3.up * (currentThrust * Time.deltaTime));
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

        //Particles/ audio

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
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            ApplyRotation(-rotationThrust);

            if (!rightThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Play();
            }
        }
        else
        {
            rightThrusterParticles.Stop();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            ApplyRotation(rotationThrust);
            
            if (!leftThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Play();
            }
        }
        else
        {
            leftThrusterParticles.Stop();
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