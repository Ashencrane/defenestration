using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

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

    [SerializeField]
    TimerManager timer;

    [SerializeField]
    AudioMixer sfxMixer;

    [SerializeField]
    AudioMixer musicMixer;

    [SerializeField]
    private GameObject[] warningBubble;

    int FADE_FACTOR = 10;
    int SCORE_TO_GET = 2;
    int P1Score;
    int P2Score;
    [SerializeField] private RawImage[] scoreGemsP1;
    [SerializeField] private RawImage[] scoreGemsP2;
    private IEnumerator coroutine;
    bool gameLive = false;
    public int currentBG;

    private void Awake()
    {
        P1m = P1.GetComponent<PlayerController>();
        P2m = P2.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {

        Display.text = "";
        Display2.text = "";
        P1Score = 0;
        P2Score = 0;
        SetMusicVol();
        SetSFXVol();


        P1m.NewRound();
        P2m.NewRound();
        Time.timeScale = 1;
        timer.ResetTimer();
        StartCoroutine("Countdown");
    }
    IEnumerator DisplayText(string text, float time, bool fadein = true)
    {
        
        Display.text = text;
        Display2.text = text;
        if (fadein)
        {
            Display.color = new Color(0f, 0f, 0f, 0f);
            Display2.color = new Color(1f, 1f, 1f, 0f);
            float fadeTime = 0f;
            for (float alpha = 0f; alpha <= 1; alpha += Time.deltaTime * FADE_FACTOR)
            {
                Display.color = new Color(0f, 0f, 0f, alpha);
                Display2.color = new Color(1f, 1f, 1f, alpha);
                fadeTime += Time.deltaTime;
                yield return null;

            }




            yield return new WaitForSeconds(time - 2 * fadeTime);

            for (float alpha2 = 1f; alpha2 >= 0; alpha2 -= Time.deltaTime * FADE_FACTOR)
            {
                Display.color = new Color(0f, 0f, 0f, alpha2);
                Display2.color = new Color(1f, 1f, 1f, alpha2);
                yield return null;

            }
        }
        else
        {
            yield return new WaitForSeconds(time);

            for (float alpha2 = 1f; alpha2 >= 0; alpha2 -= Time.deltaTime * FADE_FACTOR)
            {
                Display.color = new Color(0f, 0f, 0f, alpha2);
                Display2.color = new Color(1f, 1f, 1f, alpha2);
                yield return null;

            }
        }
        Display.color = new Color(0f, 0f, 0f, 1f);
        Display2.color = new Color(1f, 1f, 1f, 1f);

        Display.text = "";
        Display2.text = "";
        Display.fontSize = 34;
        Display2.fontSize = 34;

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameLive){
            
            if (!pauseMenu.activeInHierarchy)
            {
                //timer.ToggleTimer(false);
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                //timer.ToggleTimer(true);
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }
        }
        if (!gameLive)
        {
            P1m.actionable = false;
            P2m.actionable = false;
        }

        if (P1m.isDefenestratable && P1m.isDead == false)
        {
            warningBubble[0].SetActive(true);
        }
        else
        {
            warningBubble[0].SetActive(false);
        }
        if (P2m.isDefenestratable && P2m.isDead == false)
        {
            warningBubble[1].SetActive(true);
        }
        else
        {
            warningBubble[1].SetActive(false);
        }
        if(P1m.isDead || P2m.isDead)
        {
            timer.ToggleTimer(false);
        }
    }

    IEnumerator WaitAndStartNext() //called after someones dies
    {
        
        yield return new WaitForSeconds(0.5f);
        P1m.NewRound();
        P2m.NewRound();
        Time.timeScale = 1;
        timer.ResetTimer();

        StartCoroutine("Countdown");

    }


    IEnumerator Countdown()
    {
        
        
        yield return new WaitForSeconds(1f);
        coroutine = DisplayText("3", 0.8f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.9f);

        coroutine = DisplayText("2", 0.8f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.9f);

        coroutine = DisplayText("1", 0.8f);
        StartCoroutine(coroutine);
        yield return new WaitForSeconds(0.9f);

        P1m.actionable = true;
        P2m.actionable = true;
        timer.ToggleTimer(true);
        gameLive = true;

        coroutine = DisplayText("GO", 0.8f);
        StartCoroutine(coroutine);
        
    }

    public void SetTextKO()
    {
        coroutine = DisplayText("K.O.", 2f, false);
        StartCoroutine(coroutine);
    }

    public void SetTextDefenestration()
    {
        Display.fontSize = 18;
        Display2.fontSize = 18;
        coroutine = DisplayText("DEFENESTRATION", 2.7f, false);
        StartCoroutine(coroutine);
    }

    IEnumerator TimeOutDelay()
    {
        yield return new WaitForSeconds(2f);
        if (P1m.health > P2m.health)
        {
            RoundEnd(true);
        }
        else if (P1m.health < P2m.health)
        {
            RoundEnd(false);
        }
        else
        {
            Debug.Log(P1.transform.position.x + P2.transform.position.x);
            RoundEnd(P1.transform.position.x + P2.transform.position.x > 0);
        }
    }

    public void TimeOut()
    {
        P1m.isInvuln = true;
        P2m.isInvuln = true;
        P1m.actionable = false;
        P2m.actionable = false;
        gameLive = false;
        coroutine = DisplayText("TIME OUT", 2f);
        StartCoroutine(coroutine);
        StartCoroutine("TimeOutDelay");
    }

    //Called whenever round ends. Updates score according to who won, displays text, and calls WaitAndStartNext()
    public void RoundEnd(bool P1winner) //true = P1 won, false = P2 won
    {
        gameLive = false;
        timer.ToggleTimer(false);
        if (P1winner)
        {
            P1Score += 1;
            // -1 for 0-indexing, white color resets the increased default darkness on the gem
            scoreGemsP1[P1Score - 1].color = Color.white;
            if(P1Score < SCORE_TO_GET)
            {
                //coroutine = DisplayText("K.O.", 2f);
                StartCoroutine(coroutine);
            }
            else
            {
                Time.timeScale = 0;
                victoryMenu.GetComponent<VictoryMenu>().SetWinner(0);
                victoryMenu.GetComponent<VictoryMenu>().SetVictoryText("P1 Wins!");
                victoryMenu.SetActive(true);
            }
        }
        else
        {
            P2Score += 1;
            // -1 for 0-indexing, white color resets the increased default darkness on the gem
            scoreGemsP2[P2Score - 1].color = Color.white;
            if (P2Score < SCORE_TO_GET)
            {
                //coroutine = DisplayText("K.O.", 2f);
                StartCoroutine(coroutine);
            }
            else
            {
                Time.timeScale = 0;
                victoryMenu.GetComponent<VictoryMenu>().SetWinner(1);
                victoryMenu.GetComponent<VictoryMenu>().SetVictoryText("P2 Wins!");
                victoryMenu.SetActive(true);
            }

        }
        StartCoroutine("WaitAndStartNext");
    }
    public void SetGameLive(bool b)
    {
        gameLive = b;
    }
    private float convertToDb(float val)
    {
        return ((Mathf.Log10(val * 20) - 0.5f) * 10f);
    }

    public void SetMusicVol()
    {
        musicMixer.SetFloat("Music_Vol", convertToDb(OptionSettings.MusicSetting));
    }
    public void SetSFXVol()
    {
        sfxMixer.SetFloat("SFX_Vol", convertToDb(OptionSettings.SfxSetting));
    }


}
