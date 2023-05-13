using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    //int MAX_TIME = 60;
    int time;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        //MAX_TIME = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator TickDown()
    {

        yield return new WaitForSeconds(1f);
        time -= 1;
        text.text = time.ToString();
    }
}
