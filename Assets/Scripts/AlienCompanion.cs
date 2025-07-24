using UnityEngine;

public class AlienCompanion : MonoBehaviour
{
    [Header("Orbit Settings")]
    [Tooltip("Speed at which the companion orbits around the player (degrees per second).")]
    [SerializeField] float orbitSpeed = 20f;

    [Tooltip("Distance from the player during orbiting.")]
    [SerializeField] float orbitRadius = 2f;

    [Tooltip("Maximum vertical offset caused by noise.")]
    [SerializeField] float yAmplitude = 1f;

    [Tooltip("How quickly the vertical wobble changes over time.")]
    [SerializeField] float yNoiseSpeed = 0.2f;

    [Tooltip("Rotate clockwise if true, counter-clockwise if false.")]
    [SerializeField] bool rotateClockwise = true;


    [Header("Flip Settings")]
    [Tooltip("Chance per frame to trigger a head flip (can take values in range of 0–1).")]
    [SerializeField] float flipChance = 0.002f;

    [Tooltip("Speed at which the alien companion rotates to face the opposite direction.")]
    [SerializeField] float flipSpeed = 180f;

    Transform player;
    float angle;
    Quaternion targetRotation;
    bool isFlipping = false;
    float noiseTimeOffset;

    void Start()
    {
        var playerController = FindFirstObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene.");
            enabled = false;
            return;
        }

        player = playerController.transform;
        targetRotation = transform.rotation;
        noiseTimeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (player == null) return;

        // Determine orbit direction
        float direction = rotateClockwise ? 1f : -1f;

        // Update orbit angle
        angle += orbitSpeed * Time.deltaTime * direction;
        float rad = angle * Mathf.Deg2Rad;

        // Compute orbit position
        float x = Mathf.Cos(rad) * orbitRadius;
        float z = Mathf.Sin(rad) * orbitRadius;

        // Apply noise on Y offset
        float noiseInput = Time.time * yNoiseSpeed + noiseTimeOffset;
        float perlin = Mathf.PerlinNoise(noiseInput, 0f);
        float y = (perlin - 0.5f) * 2f * yAmplitude;

        // Apply position
        transform.position = player.position + new Vector3(x, y, z);

        // Handle flip
        if (!isFlipping && Random.value < flipChance)
        {
            isFlipping = true;
            targetRotation *= Quaternion.Euler(0f, 180f, 0f);
        }

        if (isFlipping)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, flipSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isFlipping = false;
            }
        }
    }
}
