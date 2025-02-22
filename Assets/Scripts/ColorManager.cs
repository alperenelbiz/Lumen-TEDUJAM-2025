using UnityEngine;
using System.Collections;
using UnityEngine.VFX;

public class ColorManager : SingletonBehaviour<ColorManager>
{
    [SerializeField] private VisualEffect vfx; // Assign the VFX component on "lighter"
    private bool isVFXPlaying = false; // Flag to check if the VFX is playing
    
    [SerializeField] private GameObject player; // The player object
    [SerializeField] private GameObject redObject, blueObject, greenObject; // Child objects
    [SerializeField] private GameObject lighterObject; // The lighter

    [SerializeField] private VisualEffectAsset redVFX; 
    [SerializeField] private VisualEffectAsset blueVFX; 
    [SerializeField] private VisualEffectAsset greenVFX; 
    [SerializeField] private VisualEffectAsset whiteVFX; 
    [SerializeField] private VisualEffectAsset cyanVFX; 
    [SerializeField] private VisualEffectAsset magentaVFX; 
    [SerializeField] private VisualEffectAsset yellowVFX; 

    private VisualEffectAsset targetVFX;
    private VisualEffectAsset currentVFX;
    private float fadeValue = 1f; 

    [SerializeField] private float lerpSpeed = 2f; 
    [SerializeField] private float coefficient = 0.3f;
    

    void Start()
    {
        // Ensure the player object is assigned
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // Try to find Player by tag if not assigned
            if (player == null)
            {
                Debug.LogError("Player object is missing! Assign it in the Inspector or ensure it has the 'Player' tag.");
            }
        }

        // Ensure the VFX component is assigned
        if (vfx == null)
        {
            vfx = GetComponent<VisualEffect>(); // Try to find it on the same GameObject
            if (vfx == null)
            {
                Debug.LogError("VFX Component is missing! Assign it in the Inspector.");
                return; // Exit to prevent errors
            }
        }

        // Initialize the VFX with a default effect
        if (whiteVFX != null)
        {
            vfx.visualEffectAsset = whiteVFX;
            vfx.Reinit(); // Restart the effect to apply the asset
            vfx.Play();   // Ensure the effect starts playing
        }
        else
        {
            Debug.LogError("Default whiteVFX is not assigned! Assign a default VFX asset in the Inspector.");
        }

        fadeValue = 1f; // Start with full intensity
    }
    
    void Update()
    {
        CheckColor();
    }

    void CheckColor()
    {
        bool redActive = redObject != null && redObject.transform.parent == player.transform;
        bool blueActive = blueObject != null && blueObject.transform.parent == player.transform;
        bool greenActive = greenObject != null && greenObject.transform.parent == player.transform;

        if (!redActive && !blueActive && !greenActive)
        {
            StopAllCoroutines(); // Stop any ongoing transitions
            StartCoroutine(FadeOutAndStop()); // Fade out before stopping the effect
            return;
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
        
        if (!isVFXPlaying || vfx.visualEffectAsset != targetVFX)
        {
            StopAllCoroutines(); // Ensure we don't start multiple coroutines at once
            ColorManager.Instance.StartCoroutine(SmoothVFXTransition()); // Start the transition
            isVFXPlaying = true; // Set the flag to prevent starting multiple coroutines
        }
    }
    
    IEnumerator SmoothVFXTransition()
    {
        float fadeDuration = 2f; // Duration for fade-out and fade-in
        float elapsedTime = 0f;

        while (fadeValue > 0.1f)
        {
            fadeValue = Mathf.MoveTowards(fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (vfx.HasFloat("Intensity"))
            {
                vfx.SetFloat("Intensity", fadeValue);
            }
            //elapsedTime += Time.deltaTime;
            yield return null;
        }

        vfx.visualEffectAsset = targetVFX;
        vfx.Reinit();
        vfx.Play();// Restart the effect to apply changes

        elapsedTime = 0f;

        while (fadeValue < 1f)
        {
            fadeValue = Mathf.MoveTowards(fadeValue, 1f, Time.deltaTime * lerpSpeed);
            if (vfx.HasFloat("Intensity"))
            {
                vfx.SetFloat("Intensity", fadeValue);
            }
            //elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    IEnumerator FadeOutAndStop()
    {
        while (fadeValue > 0.1f)
        {
            fadeValue = Mathf.Lerp(fadeValue, 0f, Time.deltaTime * lerpSpeed);
            if (vfx.HasFloat("Intensity"))
            {
                vfx.SetFloat("Intensity", fadeValue);
            }
            yield return null;
        }
        
        vfx.Stop();
        isVFXPlaying = false;
    }
}

