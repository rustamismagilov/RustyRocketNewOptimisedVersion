using System.Collections;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [Header("Applied to the effects at start")]
    [SerializeField] private Color portalEffectColor;

    [Header("Changing these might `break` the effects")]
    [SerializeField] private Renderer portalRenderer;
    [SerializeField] private ParticleSystem[] effectsParticles;
    [SerializeField] private Light portalLight;
    [SerializeField] private AudioSource orbAudio, flashAudio, portalAudio;

    private const float MaxVolOrb = 0.08f;
    private const float MaxVolPortal = 0.8f;
    private const float MaxLightIntensity = 4f;
    private const float TransitionSpeed = 0.3f;

    private bool inTransition, activated;
    private Material portalMat, portalEffectMat;
    private float fadeValue;

    private Coroutine transitionRoutine;

    private void Awake()
    {
        Setup();
    }

    public void TogglePortal(bool activate)
    {
        if (inTransition || activate == activated) return;

        activated = activate;

        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(activate ? PreActivate() : TransitionSequence());

        if (!activate && effectsParticles.Length > 2)
            effectsParticles[2].Stop();
    }

    private IEnumerator PreActivate()
    {
        inTransition = true;

        orbAudio.volume = MaxVolOrb;
        orbAudio.Play();
        if (effectsParticles.Length > 0)
            effectsParticles[0].Play();

        yield return new WaitForSeconds(2.2f);

        flashAudio.Play();
        portalAudio.Play();

        yield return new WaitForSeconds(0.3f);

        if (effectsParticles.Length > 2)
            effectsParticles[2].Play();

        transitionRoutine = StartCoroutine(TransitionSequence());
    }

    private IEnumerator TransitionSequence()
    {
        inTransition = true;

        while (inTransition)
        {
            if (activated)
            {
                fadeValue = Mathf.MoveTowards(fadeValue, 1f, Time.deltaTime * TransitionSpeed);
                orbAudio.volume = Mathf.Max(0, orbAudio.volume - Time.deltaTime * 0.1f);

                if (fadeValue >= 1f)
                {
                    inTransition = false;
                    orbAudio.Stop();
                }
            }
            else
            {
                fadeValue = Mathf.MoveTowards(fadeValue, 0f, Time.deltaTime * TransitionSpeed);

                if (fadeValue <= 0f)
                {
                    inTransition = false;
                    portalAudio.Stop();
                    if (effectsParticles.Length > 2)
                        effectsParticles[2].Stop();
                }
            }

            portalAudio.volume = MaxVolPortal * fadeValue;
            portalEffectMat.SetFloat("_PortalFade", fadeValue);
            portalMat.SetFloat("_EmissionStrength", fadeValue);
            portalLight.intensity = MaxLightIntensity * fadeValue;

            yield return null;
        }
    }

    private void Setup()
    {
        var mats = portalRenderer.materials;
        portalMat = mats[0];
        portalEffectMat = mats[1];

        portalMat.SetColor("_EmissionColor", portalEffectColor);
        portalMat.SetFloat("_EmissionStrength", 0f);
        portalEffectMat.SetColor("_ColorMain", portalEffectColor);
        portalEffectMat.SetFloat("_PortalFade", 0f);

        foreach (var ps in effectsParticles)
        {
            var main = ps.main;
            main.startColor = portalEffectColor;
        }

        portalAudio.volume = 0f;
        portalLight.intensity = 0f;
    }
}