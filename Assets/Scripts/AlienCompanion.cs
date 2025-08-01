using UnityEngine;

public class AlienCompanion : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] float orbitSpeed = 20f;
    [SerializeField] float orbitRadius = 2f;
    [SerializeField] float yAmplitude = 1f;
    [SerializeField] float yNoiseSpeed = 0.2f;
    [SerializeField] bool rotateClockwise = true;

    [Header("Flip Settings")]
    [SerializeField] float flipChance = 0.002f;
    [SerializeField] float flipSpeed = 180f;

    [Header("Combat Settings")]
    [SerializeField] Transform combatOffsetPoint;
    [SerializeField] SphereCollider detectionTrigger;
    [SerializeField] float targetRotationSpeed = 5f;

    [Header("Audio Settings")]
    [SerializeField] AudioClip detectionSound;
    [SerializeField] float detectionVolume = 0.4f;

    Shooter shooter;

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
    bool soundPlayed = false;

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

        if (detectionSound != null && !soundPlayed)
        {
            AudioSource.PlayClipAtPoint(detectionSound, transform.position, detectionVolume);
            soundPlayed = true;
        }
    }

    public void SetCombatConfig(Transform target, float shootSpeedMultiplier, float damageMultiplier)
    {
        combatMode = target != null;
        currentTarget = target;
        currentShootIntervalMultiplier = shootSpeedMultiplier;
        currentDamageMultiplier = damageMultiplier;

        if (detectionSound != null && !soundPlayed)
        {
            AudioSource.PlayClipAtPoint(detectionSound, transform.position, detectionVolume);
            soundPlayed = true;
        }
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
