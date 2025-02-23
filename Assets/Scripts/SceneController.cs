using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // Assign in Inspector
    public float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1;
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeCanvas.blocksRaycasts = false;
    }

    private IEnumerator FadeOut()
    {
        fadeCanvas.blocksRaycasts = true;
        while (fadeCanvas.alpha < 1)
        {
            fadeCanvas.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Ensure the player has the "Player" tag
        {
            StartCoroutine(FadeOut());
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Check if the next scene index is within range
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.Log("No more scenes in build settings.");
            }
        }
    }
}
