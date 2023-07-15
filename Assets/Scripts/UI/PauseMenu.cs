using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameController gc;
    public void Unpause()
    {
        // add unpause code here before the following line
        gc.TogglePause(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
