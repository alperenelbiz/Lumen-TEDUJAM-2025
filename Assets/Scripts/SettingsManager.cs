using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsManager : MonoBehaviour
{
    [SerializeField] Image SoundOnIcon;
    [SerializeField] Image SoundOffIcon;
    private bool muted = false;

    void Start()
    {
        Load();
        UpdateButtonIcon();
    }
    void UpdateButtonIcon()
    {
        if(muted == false)
        {
            SoundOffIcon.enabled = false;
            SoundOnIcon.enabled = true;
        }

        else
        {
            SoundOffIcon.enabled = true;
            SoundOnIcon.enabled = false;
        }
    }
    public void OnMusicButtonPress()
    {
        if(muted == false)
        {
            muted = true;
            AudioManager.Instance.PauseAllSounds();
        }
        else
        {
            muted = false;
            AudioManager.Instance.ResumeAllSounds();
        }
        Save();
        UpdateButtonIcon();
    }

    private void Load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }
    private void Save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0);
    }
}
