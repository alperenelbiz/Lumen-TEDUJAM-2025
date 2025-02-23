using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // Assign in Inspector
    public float fadeDuration = 1f;

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial"); // Replace with your game scene name
    }

    public void OpenLevels()
    {
        SceneManager.LoadScene("Levels"); // Replace with your levels menu scene name
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // Replace with your settings scene name
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits"); // Replace with your credits scene name
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace with your credits scene name
    }

    public void QuitGame()
    {
        Application.Quit(); // Works in a build; won’t work in the Unity editor
    }
}
