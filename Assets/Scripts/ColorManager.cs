using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class ColorManager : MonoBehaviour
{
    [System.Serializable]
    public class Lighter
    {
        public GameObject lighterObject;
        public VisualEffect vfxComponent;
        public GameObject parent;
        public float fadeValue = 1f;
    }

    [SerializeField] private List<Lighter> lighters = new List<Lighter>();
    private Dictionary<Lighter, VisualEffectAsset> lastAppliedVFX = new Dictionary<Lighter, VisualEffectAsset>();

    [SerializeField] private GameObject redObject, blueObject, greenObject;

    [SerializeField] private VisualEffectAsset redVFX;
    [SerializeField] private VisualEffectAsset blueVFX;
    [SerializeField] private VisualEffectAsset greenVFX;
    [SerializeField] private VisualEffectAsset whiteVFX;
    [SerializeField] private VisualEffectAsset cyanVFX;
    [SerializeField] private VisualEffectAsset magentaVFX;
    [SerializeField] private VisualEffectAsset yellowVFX;

    private Dictionary<Lighter, Coroutine> activeCoroutines = new Dictionary<Lighter, Coroutine>();
    //private float fadeValue = 1f;
    [SerializeField] private float lerpSpeed = 2f;

    void Start()
    {
        foreach (var lighter in lighters)
        {
            if (lighter.vfxComponent == null)
            {
                //Debug.LogError($"VFX Component is missing on {lighter.lighterObject.name}! Assign it in the Inspector.");
                continue;
            }
            if (whiteVFX != null)
            {
                lighter.vfxComponent.visualEffectAsset = whiteVFX;
                lighter.vfxComponent.Reinit();
                lighter.vfxComponent.Play();
            }
        }
    }

    void Update()
    {
        CheckColor();
    }

    void CheckColor()
    {
        foreach (var lighter in lighters)
        {
            bool redActive = redObject != null && redObject.transform.IsChildOf(lighter.parent.transform);
            bool blueActive = blueObject != null && blueObject.transform.IsChildOf(lighter.parent.transform);
            bool greenActive = greenObject != null && greenObject.transform.IsChildOf(lighter.parent.transform);

            Debug.Log($"Lighter: {lighter.lighterObject.name} | Parent: {lighter.parent.name} | RedObject Parent: {redObject?.transform.parent?.name}");
            Debug.Log($"Checking if {redObject?.name} is child of {lighter.parent.name}: {redObject?.transform.IsChildOf(lighter.parent.transform)}");
            Debug.Log($"Checking if {blueObject?.name} is child of {lighter.parent.name}: {blueObject?.transform.IsChildOf(lighter.parent.transform)}");
            Debug.Log($"Checking if {greenObject?.name} is child of {lighter.parent.name}: {greenObject?.transform.IsChildOf(lighter.parent.transform)}");
            VisualEffectAsset targetVFX = null;

            if (!redActive && !blueActive && !greenActive)
            {
                if (activeCoroutines.ContainsKey(lighter) && activeCoroutines[lighter] != null)
                {
                    StopCoroutine(activeCoroutines[lighter]);
                }
                activeCoroutines[lighter] = StartCoroutine(FadeOutAndStop(lighter));
                lastAppliedVFX[lighter] = null; 
                continue;
            }

            if (redActive && blueActive && greenActive)
            {
                targetVFX = whiteVFX;
            }

            else if (redActive && blueActive && !greenActive)
            {
                targetVFX = magentaVFX;
            }
            else if (blueActive && greenActive && !redActive)
            {
                targetVFX = cyanVFX;
            }
            else if (redActive && greenActive && !blueActive)
            {
                targetVFX = yellowVFX;
            }
            else if (redActive && !blueActive && !greenActive)
            {
                targetVFX = redVFX;
            }
            else if (blueActive && !redActive && !greenActive)
            {
                targetVFX = blueVFX;
            }
            else if (greenActive && !redActive && !blueActive)
            {
                targetVFX = greenVFX;
            }

            if (!lastAppliedVFX.ContainsKey(lighter) || lastAppliedVFX[lighter] != targetVFX)
            {
                if (activeCoroutines.ContainsKey(lighter) && activeCoroutines[lighter] != null)
                {
                    StopCoroutine(activeCoroutines[lighter]);
                    activeCoroutines.Remove(lighter);
                }
                lastAppliedVFX[lighter] = targetVFX;
                activeCoroutines[lighter] = StartCoroutine(SmoothVFXTransition(lighter, targetVFX));
            }
        }
    }

    IEnumerator SmoothVFXTransition(Lighter lighter, VisualEffectAsset targetVFX)
    {
        while (lighter.fadeValue > 0.1f)
        {
            lighter.fadeValue = Mathf.MoveTowards(lighter.fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", lighter.fadeValue);
            }
            yield return null;
        }

        lighter.vfxComponent.visualEffectAsset = targetVFX;
        lighter.vfxComponent.Reinit();
        lighter.vfxComponent.Play();

        while (lighter.fadeValue < 1f)
        {
            lighter.fadeValue = Mathf.MoveTowards(lighter.fadeValue, 1f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", lighter.fadeValue);
            }
            yield return null;
        }
    }

    IEnumerator FadeOutAndStop(Lighter lighter)
    {
        while (lighter.fadeValue > 0.1f)
        {
            lighter.fadeValue = Mathf.Lerp(lighter.fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", lighter.fadeValue);
            }
            yield return null;
        }
        lighter.vfxComponent.Stop();
    }
}