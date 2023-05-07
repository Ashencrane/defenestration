using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject P1;
    public GameObject P2;
    Movement P1m;
    Movement P2m;
    public TMP_Text Display;
    public TMP_Text Display2;

    int P1Score;
    int P2Score;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        P1m = P1.GetComponent<Movement>();
        P2m = P2.GetComponent<Movement>();
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
            coroutine = DisplayText("P1 score: " + P1Score, 2f);
            StartCoroutine(coroutine);
        }
        else
        {
            P2Score += 1;
            coroutine = DisplayText("P2 score:" + P2Score, 2f);
            StartCoroutine(coroutine);
        }
        StartCoroutine("WaitAndStartNext");
    }

}
