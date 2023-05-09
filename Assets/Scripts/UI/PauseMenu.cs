using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Unpause()
    {
        // add unpause code here before the following line
        gameObject.SetActive(false);  // turn off pause menu
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
