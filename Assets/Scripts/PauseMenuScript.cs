using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private bool pause = false;
    [SerializeField] private GameObject PauseMenu;

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (!pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        pause = true;
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        pause = false;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
