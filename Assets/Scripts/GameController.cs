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

    IEnumerator WaitAndStartNext()
    {
        yield return new WaitForSeconds(2f);
        P1m.NewRound();
        P2m.NewRound();
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
                victoryMenu.GetComponent<VictoryMenu>().SetVictoryText("P2 Wins!");
                victoryMenu.SetActive(true);
            }

        }
        StartCoroutine("WaitAndStartNext");
    }


}
