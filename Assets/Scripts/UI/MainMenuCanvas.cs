using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
}
