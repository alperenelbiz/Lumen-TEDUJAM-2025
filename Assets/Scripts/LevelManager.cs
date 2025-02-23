using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level 01");
    }
    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level 02");
    }
    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level 03");
    }
    public void OpenLevel4()
    {
        SceneManager.LoadScene("Level 04");
    }
}
