using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.VFX;

public class ColorManager : SingletonBehaviour<ColorManager>
{
    [System.Serializable]
    public class Lighter
    {
        public GameObject lighterObject;
        public VisualEffect vfxComponent;
        public GameObject parent;
    }
    
    [SerializeField] private List<Lighter> lighters = new List<Lighter>();
    
    [SerializeField] private GameObject redObject, blueObject, greenObject;
    
    [SerializeField] private VisualEffectAsset redVFX;
    [SerializeField] private VisualEffectAsset blueVFX;
    [SerializeField] private VisualEffectAsset greenVFX;
    [SerializeField] private VisualEffectAsset whiteVFX;
    [SerializeField] private VisualEffectAsset cyanVFX;
    [SerializeField] private VisualEffectAsset magentaVFX;
    [SerializeField] private VisualEffectAsset yellowVFX;
    
    private Dictionary<Lighter, Coroutine> activeCoroutines = new Dictionary<Lighter, Coroutine>();
    private float fadeValue = 1f;
    [SerializeField] private float lerpSpeed = 2f;
    
    void Start()
    {
        foreach (var lighter in lighters)
        {
            if (lighter.vfxComponent == null)
            {
                Debug.LogError($"VFX Component is missing on {lighter.lighterObject.name}! Assign it in the Inspector.");
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
            bool redActive = redObject != null && redObject.transform.parent == lighter.parent.transform;
            bool blueActive = blueObject != null && blueObject.transform.parent == lighter.parent.transform;
            bool greenActive = greenObject != null && greenObject.transform.parent == lighter.parent.transform;

            VisualEffectAsset targetVFX = null;
            
            if (!redActive && !blueActive && !greenActive)
            {
                if (activeCoroutines.ContainsKey(lighter) && activeCoroutines[lighter] != null)
                {
                    StopCoroutine(activeCoroutines[lighter]);
                }
                activeCoroutines[lighter] = StartCoroutine(FadeOutAndStop(lighter));
                continue;
            }

            if (redActive && blueActive && greenActive)
            {
                targetVFX = whiteVFX;
            }
            else if (redActive && blueActive)
            {
                targetVFX = magentaVFX;
            }
            else if (blueActive && greenActive)
            {
                targetVFX = cyanVFX;
            }
            else if (redActive && greenActive)
            {
                targetVFX = yellowVFX;
            }
            else if (redActive)
            {
                targetVFX = redVFX;
            }
            else if (blueActive)
            {
                targetVFX = blueVFX;
            }
            else if (greenActive)
            {
                targetVFX = greenVFX;
            }

            if (lighter.vfxComponent.visualEffectAsset != targetVFX)
            {
                if (activeCoroutines.ContainsKey(lighter) && activeCoroutines[lighter] != null)
                {
                    StopCoroutine(activeCoroutines[lighter]);
                }
                activeCoroutines[lighter] = StartCoroutine(SmoothVFXTransition(lighter, targetVFX));
            }
        }
    }

    IEnumerator SmoothVFXTransition(Lighter lighter, VisualEffectAsset targetVFX)
    {
        while (fadeValue > 0.1f)
        {
            fadeValue = Mathf.MoveTowards(fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", fadeValue);
            }
            yield return null;
        }

        lighter.vfxComponent.visualEffectAsset = targetVFX;
        lighter.vfxComponent.Reinit();
        lighter.vfxComponent.Play();

        while (fadeValue < 1f)
        {
            fadeValue = Mathf.MoveTowards(fadeValue, 1f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", fadeValue);
            }
            yield return null;
        }
    }

    IEnumerator FadeOutAndStop(Lighter lighter)
    {
        while (fadeValue > 0.1f)
        {
            fadeValue = Mathf.Lerp(fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (lighter.vfxComponent.HasFloat("Intensity"))
            {
                lighter.vfxComponent.SetFloat("Intensity", fadeValue);
            }
            yield return null;
        }
        
        lighter.vfxComponent.Stop();
    }
}
