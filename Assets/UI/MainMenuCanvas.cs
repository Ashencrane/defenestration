using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    [SerializeField] private string LevelName;

    public void OnPressStartButton()
    {
        SceneManager.LoadScene(LevelName);
    }
    
    public void OnPressQuitButton()
    {
        Application.Quit();
    }
}
