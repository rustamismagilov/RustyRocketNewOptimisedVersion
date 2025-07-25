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

    [SerializeField] Transform combatOffsetPoint;
    [SerializeField] SphereCollider detectionTrigger;
    Shooter shooter;

    [SerializeField] float targetRotationSpeed = 5f;

    bool combatMode = false;
    Transform currentTarget;
    float shootCooldown = 0f;
    float currentShootIntervalMultiplier = 1f;
    float currentDamageMultiplier = 1f;

    Transform player;
    float angle;
    Quaternion targetRotation;
    bool isFlipping = false;
    float noiseTimeOffset;

    void Start()
    {
        shooter = FindFirstObjectByType<PlayerController>().GetComponent<Shooter>();

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

        if (combatOffsetPoint == null)
            Debug.LogWarning("Combat offset point not assigned to AlienCompanion.");

        if (detectionTrigger == null)
            Debug.LogWarning("Detection trigger not assigned to AlienCompanion.");
    }

    void Update()
    {
        if (player == null) return;

        if (combatMode && currentTarget != null)
            HandleCombat();
        else
            HandleOrbit();
    }

    void HandleOrbit()
    {
        // Determine orbit direction
        float direction = rotateClockwise ? 1f : -1f;

        angle += orbitSpeed * Time.deltaTime * direction;
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad) * orbitRadius;
        float z = Mathf.Sin(rad) * orbitRadius;

        float noiseInput = Time.time * yNoiseSpeed + noiseTimeOffset;
        float perlin = Mathf.PerlinNoise(noiseInput, 0f);
        float y = (perlin - 0.5f) * 2f * yAmplitude;

        transform.position = player.position + new Vector3(x, y, z);

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

    public void SetCombatTarget(Transform target)
    {
        combatMode = target != null;
        currentTarget = target;
    }

    public void SetCombatConfig(Transform target, float shootSpeedMultiplier, float damageMultiplier)
    {
        combatMode = target != null;
        currentTarget = target;

        currentShootIntervalMultiplier = shootSpeedMultiplier;
        currentDamageMultiplier = damageMultiplier;
    }


    void HandleCombat()
    {
        if (combatOffsetPoint != null)
            transform.position = combatOffsetPoint.position;

        if (currentTarget != null)
        {
            Vector3 dir = currentTarget.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, targetRotationSpeed * Time.deltaTime);

            shootCooldown -= Time.deltaTime;
            if (shootCooldown <= 0f)
            {
                int finalDamage = Mathf.RoundToInt(shooter.GetDamage() * currentDamageMultiplier);
                shooter.Shoot(finalDamage);
                shootCooldown = shooter.GetShootInterval() / currentShootIntervalMultiplier;
            }
        }
    }
}
