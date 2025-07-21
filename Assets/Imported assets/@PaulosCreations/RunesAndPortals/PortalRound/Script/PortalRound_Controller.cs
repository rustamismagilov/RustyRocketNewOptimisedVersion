using System.Collections;
using UnityEngine;

public class PortalRoundController : MonoBehaviour
{
    [Header("Applied to the effects at start")]
    [SerializeField] private Color effectsColor;

    [Header("Changing these might `break` the effects")]
    [SerializeField] private ParticleSystem[] effectsPartSystems;
    [SerializeField] private Light portalLight;
    [SerializeField] private Transform portalRoundMeshTF;
    [SerializeField] private AudioSource portalAudio;

    [Space(10)]
    [SerializeField] private bool floatingAnimationOn = true;
    [SerializeField] private AnimationCurve floatingCurve;

    private bool portalActive, inTransition, isFloating;
    private float transitionValue, baseLightIntensity, evalFloat, floatSpeed = 0.2f;
    private Material portalMaterial;
    private Vector3 originalPosition;

    private Coroutine transitionRoutine, floatingRoutine;

    private void OnEnable()
    {
        originalPosition = transform.position;
        portalMaterial = portalRoundMeshTF.GetComponent<Renderer>().material;
        portalMaterial.SetColor("_EmissionColor", effectsColor);
        portalMaterial.SetFloat("_EmissionStrength", 0);

        portalLight.color = effectsColor;
        baseLightIntensity = portalLight.intensity;
        portalLight.intensity = 0;

        foreach (var ps in effectsPartSystems)
        {
            var main = ps.main;
            main.startColor = effectsColor;
        }
    }

    public void TogglePortalRound(bool activate)
    {
        if (portalActive == activate) return;

        portalActive = activate;

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(PortalTransition());

        if (activate)
        {
            foreach (var ps in effectsPartSystems)
                ps.Play();

            portalAudio.Play();

            if (floatingAnimationOn && !isFloating)
                floatingRoutine = StartCoroutine(FloatingMovement());
        }
        else
        {
            foreach (var ps in effectsPartSystems)
                ps.Stop();

            if (isFloating)
            {
                StopCoroutine(floatingRoutine);
                isFloating = false;
            }
        }
    }

    private IEnumerator PortalTransition()
    {
        inTransition = true;

        if (portalActive)
        {
            while (transitionValue < 1f)
            {
                transitionValue = Mathf.MoveTowards(transitionValue, 1f, Time.deltaTime * 0.1f);
                UpdatePortalVisuals();
                yield return null;
            }
        }
        else
        {
            while (transitionValue > 0f)
            {
                transitionValue = Mathf.MoveTowards(transitionValue, 0f, Time.deltaTime * 0.4f);
                UpdatePortalVisuals();
                yield return null;
            }

            portalAudio.Stop();
        }

        inTransition = false;
    }

    private void UpdatePortalVisuals()
    {
        portalMaterial.SetFloat("_EmissionStrength", transitionValue);
        portalLight.intensity = baseLightIntensity * transitionValue;
        portalAudio.volume = transitionValue * 0.8f;
    }

    private IEnumerator FloatingMovement()
    {
        isFloating = true;

        while (true)
        {
            evalFloat += Time.deltaTime * floatSpeed;
            if (evalFloat >= 1f) evalFloat = 0f;

            var offset = floatingCurve.Evaluate(evalFloat);
            transform.position = originalPosition + Vector3.up * offset;

            yield return null;
        }
    }
}
