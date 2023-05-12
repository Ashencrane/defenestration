using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    PlayerController P1m;
    PlayerController P2m;
    
    [SerializeField]
    TimerManager tm;

    public TMP_Text Display;
    public TMP_Text Display2;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject victoryMenu;

    int SCORE_TO_GET = 2;
    int P1Score;
    int P2Score;
    private IEnumerator coroutine;



    // Start is called before the first frame update
    void Start()
    {
        P1m = P1.GetComponent<PlayerController>();
        P2m = P2.GetComponent<PlayerController>();
        Display.text = "";
        Display2.text = "";
        P1Score = 0;
        P2Score = 0;

        P1m.NewRound();
        P2m.NewRound();
        StartCoroutine("Countdown");
    }
    IEnumerator DisplayText(string text, float time)
    {
        Display.text = text;
        Display2.text = text;
        yield return new WaitForSeconds(time);
        Display.text = "";
        Display2.text = "";
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            
            if (!pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator WaitAndStartNext() //called after someones dies
    {
        yield return new WaitForSeconds(2f);
        P1m.NewRound();
        P2m.NewRound();

        StartCoroutine("Countdown");

    }

    IEnumerator Countdown()
    {
        coroutine = DisplayText("3", 0.7f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.8f);

        coroutine = DisplayText("2", 0.7f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.8f);

        coroutine = DisplayText("1", 0.7f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.8f);

        P1m.actionable = true;
        P2m.actionable = true;
        coroutine = DisplayText("GO", 0.7f);
        StartCoroutine(coroutine);
    }

    public void RoundEnd(bool P1winner) //true = P1 won, false = P2 won
    {
        if (P1winner)
        {
            P1Score += 1;
            if(P1Score < SCORE_TO_GET)
            {
                coroutine = DisplayText("P1 score: " + P1Score, 2f);
                StartCoroutine(coroutine);
            }
            else
            {
                Time.timeScale = 0;
                victoryMenu.GetComponent<VictoryMenu>().SetVictoryText("P1 Wins!");
                victoryMenu.SetActive(true);
            }
        }
        else
        {
            P2Score += 1;
            if (P2Score < SCORE_TO_GET)
            {
                coroutine = DisplayText("P2 score: " + P2Score, 2f);
                StartCoroutine(coroutine);
            }
            else
            {
                Time.timeScale = 0;
                victoryMenu.GetComponent<VictoryMenu>().SetVictoryText("P2 Wins!");
                victoryMenu.SetActive(true);
            }

        }
        StartCoroutine("WaitAndStartNext");
    }


}
