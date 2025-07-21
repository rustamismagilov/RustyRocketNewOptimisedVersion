using System.Collections;
using UnityEngine;

public class PortalGateController : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private Color portalColor;
    [SerializeField] private Renderer portalRenderer;
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private Light portalLight;
    [SerializeField] private Transform symbol;
    [SerializeField] private AudioSource portalAudio;
    [SerializeField] private AudioSource flashAudio;

    private Material portalMat, portalEffectMat;
    private Vector3 symbolStartPos;

    private bool portalActive, inTransition;
    private float transitionProgress, initialLightIntensity;

    private Coroutine transitionRoutine, symbolRoutine;

    void OnEnable()
    {
        var mats = portalRenderer.materials;
        portalMat = mats[0];
        portalEffectMat = mats[1];

        portalMat.SetColor("_EmissionColor", portalColor);
        portalMat.SetFloat("_EmissionStrength", 0);
        portalEffectMat.SetColor("_ColorMain", portalColor);
        portalEffectMat.SetFloat("_PortalFade", 0);

        portalLight.color = portalColor;
        initialLightIntensity = portalLight.intensity;
        portalLight.intensity = 0;

        symbolStartPos = symbol.localPosition;
        symbol.GetComponent<Renderer>().material = portalMat;

        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.startColor = portalColor;
        }
    }

    public void TogglePortal(bool activate)
    {
        if (inTransition || portalActive == activate) return;

        portalActive = activate;

        foreach (var ps in particleSystems)
        {
            if (activate) ps.Play(); else ps.Stop();
        }

        if (activate)
        {
            portalAudio.Play();
            flashAudio.Play();
            if (symbolRoutine != null) StopCoroutine(symbolRoutine);
            symbolRoutine = StartCoroutine(AnimateSymbol());
        }

        if (transitionRoutine != null) StopCoroutine(transitionRoutine);
        transitionRoutine = StartCoroutine(AnimatePortal());
    }

    IEnumerator AnimatePortal()
    {
        inTransition = true;
        float speed = portalActive ? 0.2f : 0.4f;
        float target = portalActive ? 1f : 0f;

        while (!Mathf.Approximately(transitionProgress, target))
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, target, Time.deltaTime * speed);
            portalMat.SetFloat("_EmissionStrength", transitionProgress);
            portalEffectMat.SetFloat("_PortalFade", transitionProgress * 0.4f);
            portalLight.intensity = initialLightIntensity * transitionProgress;
            portalAudio.volume = transitionProgress * 0.8f;
            yield return null;
        }

        if (!portalActive)
        {
            portalAudio.Stop();
            if (symbolRoutine != null) StopCoroutine(symbolRoutine);
        }

        inTransition = false;
    }

    IEnumerator AnimateSymbol()
    {
        Vector3 targetPos = symbolStartPos;
        float lerpSpeed = 0.001f;

        while (true)
        {
            if (symbol.localPosition == targetPos)
            {
                targetPos = symbolStartPos + new Vector3(0, Random.Range(-0.08f, 0.08f), Random.Range(-0.08f, 0.08f));
            }
            else
            {
                symbol.localPosition = Vector3.Slerp(symbol.localPosition, targetPos, lerpSpeed);
            }

            yield return new WaitForSeconds(0.04f);
        }
    }
}
