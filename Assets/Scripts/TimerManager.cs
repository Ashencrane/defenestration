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

    private void Awake()
    {
        textDisplay = gameObject.GetComponent<TMP_Text>();
    }
    void Start()
    {
        MAX_TIME = 60;
        paused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            time -= Time.deltaTime;
            textDisplay.text = ((int)time).ToString();
        }
        
        
    }
    public void ResetTimer()
    {
        time = MAX_TIME;
    }
    public void ToggleTimer(bool play)
    {
        paused = !play;
    }
}
