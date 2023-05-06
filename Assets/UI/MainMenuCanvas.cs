using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

struct MainMenuSettings
{
    public static float MusicSetting = 1;
    public static float SfxSetting = 1;
}

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private string levelName;

    public void OnPressStartButton()
    {
        SceneManager.LoadScene(levelName);
    }
    
    public void OnPressQuitButton()
    {
        Application.Quit();
    }

    public void UpdateMusicSetting(float val)
    {
        MainMenuSettings.MusicSetting = val;
        Debug.Log("New music volume: " + MainMenuSettings.MusicSetting);
    }

    public void UpdateSfxSetting(float val)
    {
        MainMenuSettings.SfxSetting = val;
        Debug.Log("New sfx volume: " + MainMenuSettings.SfxSetting);
    }
}
