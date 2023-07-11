using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    int MAX_TIME;
    float time;
    TMP_Text textDisplay;
    bool paused;
    bool timeOut;

    [SerializeField]
    GameController gameController;

    private void Awake()
    {
        textDisplay = gameObject.GetComponent<TMP_Text>();
    }
    void Start()
    {
        MAX_TIME = 60;
        paused = true;
        timeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            time -= Time.deltaTime;
            textDisplay.text = ((int)time).ToString();
            if (time <= 0 && timeOut == false)
            {
                Debug.Log("timed out!");
                timeOut = true;
                paused = true;
                gameController.TimeOut();
            }
        }
        
        
    }
    public void ResetTimer()
    {
        //Debug.Log("ResetTimer");
        time = MAX_TIME;
        textDisplay.text = ((int)time).ToString();
        timeOut = false;
    }
    public void ToggleTimer(bool play)
    {
        paused = !play;
    }
}
