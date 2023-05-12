using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerController : MonoBehaviour
{

    //GameObject sword;
    Rigidbody2D rb2d;
    Slider healthDisplay;
    PlayerController otherPlayerController;
    SpriteRenderer spr;
    GameController gameController;
    AudioManager audioMan;


    double forwarDashSec = 0;
    double backDashSec = 0;

    public bool P1;
    public int health;

    const int MAX_HEALTH = 8;
    const float MOVE_SPEED = 4.5f;

    const float STARTING_DISTANCE = 6f;

    const float NONE_ATTACK_KNOCKBACK = 30f;

    const float LIGHT_ATTACK_STARTUP = 0.1f;
    const float LIGHT_ATTACK_ACTIVE = 0.2f;
    const float LIGHT_ATTACK_RECOVERY = 0.15f;
    const float LIGHT_ATTACK_DISTANCE = 1.1f;
    const float LIGHT_ATTACK_STUN = 0.25f;
    const float LIGHT_ATTACK_KNOCKBACK = 180f;

    const float HEAVY_ATTACK_STARTUP = 0.3f;
    const float HEAVY_ATTACK_ACTIVE = 0.3f;
    const float HEAVY_ATTACK_RECOVERY = 0.7f;
    const float HEAVY_ATTACK_DISTANCE = 1.6f;
    const float HEAVY_ATTACK_STUN = 0.45f;
    const float HEAVY_ATTACK_KNOCKBACK = 280f;

    const float FORWARDASH_SPEED = 8f;
    const float FORWARDASH_ACTIVE = 0.25f;
    const float FORWARDASH_RECOVERY = 0.10f;

    const float BACKDASH_SPEED = 8f;
    const float BACKDASH_ACTIVE = 0.25f;
    const float BACKDASH_RECOVERY = 0.15f;

    const float LUNGE_FORCE = 350f;

    public enum Attack
    {
        None, Light, Heavy
    }
    public Attack currentAttack;


    public bool actionable = true;
    bool inHitstun = false;
    int direction;

    [SerializeField]
    AnimationManager animationManager;
    


    // Start is called before the first frame update
    void Start()
    {
        
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();
        audioMan = GameObject.Find("Main Camera").GetComponent<AudioManager>();

        if (P1) 
        {
            healthDisplay = GameObject.Find("/Canvas/P1Health/P1HealthOutline").GetComponent<Slider>();
            //sword = GameObject.Find("/Player1/P1Sword");
            direction = 1;
            otherPlayerController = GameObject.Find("Player2").GetComponent<PlayerController>();
            
        } 
        else 
        {
            healthDisplay = GameObject.Find("/Canvas/P2Health/P2HealthOutline").GetComponent<Slider>();
            //sword = GameObject.Find("/Player2/P2Sword");
            direction = -1;
            otherPlayerController = GameObject.Find("Player1").GetComponent<PlayerController>();
        }
        NewRound();
    }
    //move forward
    //move backward
    //light attack
    //heavy attack
    //shield

    // Update is called once per frame
    void Update()
    {
        if (backDashSec > 0)
        {
            backDashSec -= Time.deltaTime;
        }
        if (forwarDashSec > 0)
        {
            forwarDashSec -= Time.deltaTime;
        }


        if (actionable)
        {
            //backdash code
            
            if (Input.GetKeyDown(KeyCode.A) && P1 || Input.GetKeyDown(KeyCode.RightArrow) && !P1) //scuffed but works
            {
                if (backDashSec > 0)
                {
                    StartCoroutine("Backdash");
                    backDashSec = 0;
                }
                else
                {
                    backDashSec = 0.25f;
                }
            }

            //forwardash code

            if (Input.GetKeyDown(KeyCode.D) && P1 || Input.GetKeyDown(KeyCode.LeftArrow) && !P1) //scuffed but works
            {
                if (forwarDashSec > 0)
                {
                    StartCoroutine("Forwardash");
                    forwarDashSec = 0;
                }
                else
                {
                    forwarDashSec = 0.25f;
                }
            }

            int leftright = P1 ? (int)Input.GetAxisRaw("HorizontalP1") : (int)Input.GetAxisRaw("HorizontalP2");


            if (leftright == -1)
            {
                gameObject.transform.Translate(new Vector2(-MOVE_SPEED, 0) * Time.deltaTime);
            }
            else if (leftright == 1)
            {
                gameObject.transform.Translate(new Vector2(MOVE_SPEED, 0) * Time.deltaTime);
            }

            if (P1 ? (int)Input.GetAxisRaw("LightP1") == 1 : (int)Input.GetAxisRaw("LightP2") == 1)
            {
                StartCoroutine("LightAttack");
            }

            if (P1 ? (int)Input.GetAxisRaw("HeavyP1") == 1 : (int)Input.GetAxisRaw("HeavyP2") == 1)
            {
                StartCoroutine("HeavyAttack");
            }


        }
    }
    public void NewRound()
    {
        animationManager.Reset();
        transform.position = new Vector3(STARTING_DISTANCE * -direction, 0, 0);
        health = MAX_HEALTH;
        currentAttack = Attack.None;
        healthDisplay.value = (float)health / MAX_HEALTH;
    }

    public void Lunge()
    {
        rb2d.AddForce(new Vector2(LUNGE_FORCE * direction, 0));
    }


    IEnumerator Backdash()
    {
        actionable = false;
        spr.color = new Color(0.7f, 0.7f, 0.7f);
        rb2d.drag = 0;
        rb2d.velocity = new Vector3(BACKDASH_SPEED * -direction, 0, 0);

        yield return new WaitForSeconds(BACKDASH_ACTIVE);
        rb2d.velocity = new Vector3(0, 0, 0);
        rb2d.drag = 0.05f;

        yield return new WaitForSeconds(BACKDASH_RECOVERY);
        
        spr.color = new Color(200, 200, 200);
        actionable = true;
        

        yield return null;
    }

    IEnumerator Forwardash()
    {
        actionable = false;
        spr.color = new Color(0.7f, 0.7f, 0.7f);
        rb2d.velocity = new Vector3(FORWARDASH_SPEED * direction, 0, 0);
        rb2d.drag = 0;
        
        
        yield return new WaitForSeconds(FORWARDASH_ACTIVE);
        rb2d.drag = 0.05f;
        rb2d.velocity = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(FORWARDASH_RECOVERY);

        spr.color = new Color(200, 200, 200);
        actionable = true;

        yield return null;
    }

    IEnumerator LightAttack()
    {
        animationManager.LightAttack();
        currentAttack = Attack.Light;
        /*
        actionable = false;
        yield return new WaitForSeconds(LIGHT_ATTACK_STARTUP);
        //sword.transform.Translate(new Vector2(LIGHT_ATTACK_DISTANCE * direction, 0)); //stab extend
        spr.color = new Color(0.7f, 0.7f, 0.7f);

        yield return new WaitForSeconds(LIGHT_ATTACK_ACTIVE);
        //sword.transform.Translate(new Vector2(-LIGHT_ATTACK_DISTANCE * direction, 0));

        yield return new WaitForSeconds(LIGHT_ATTACK_RECOVERY);
        spr.color = new Color(200, 200, 200);

        currentAttack = Attack.None; 
        actionable = true;
        */
        yield return null;
        
    }

    IEnumerator HeavyAttack()
    {
        animationManager.HeavyAttack();
        currentAttack = Attack.Heavy;
        /*
        actionable = false;
        spr.color = new Color(0.3f, 0.3f, 1f);

        yield return new WaitForSeconds(HEAVY_ATTACK_STARTUP);
        //sword.transform.Translate(new Vector2(HEAVY_ATTACK_DISTANCE * direction, 0)); //stab extend
        spr.color = new Color(0.7f, 0.7f, 0.7f);

        yield return new WaitForSeconds(HEAVY_ATTACK_ACTIVE);
        //sword.transform.Translate(new Vector2(-HEAVY_ATTACK_DISTANCE * direction, 0));

        yield return new WaitForSeconds(HEAVY_ATTACK_RECOVERY * direction);
        spr.color = new Color(200, 200, 200);


        currentAttack = Attack.None;
        actionable = true;
        */
        yield return null;
    }

    public void OnHit(Hitbox hitbox, Hitbox colHitbox)
    {
        
        if (colHitbox.GetBoxType() == Hitbox.BoxType.Hit && !inHitstun && colHitbox.isActive)
        {
            if (colHitbox.attackType == Hitbox.AttackType.Light)
            {
                //UnityEngine.Debug.Log("light");
                StartCoroutine("HitByLight");
            }
            else if (colHitbox.attackType == Hitbox.AttackType.Heavy)
            {
                //UnityEngine.Debug.Log("heavy");
                StartCoroutine("HitByHeavy");
            }
            else
            {
                //UnityEngine.Debug.Log("none");
                StartCoroutine("HitByNone");
            }
        }
    }
    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if(P1 && col.tag == "HitboxP2" || !P1 && col.tag == "HitboxP1") 
        {
            Debug.Log(otherPlayerController.currentAttack);
            if(otherPlayerController.currentAttack == Attack.Light)
            {
                StartCoroutine("HitByLight");
            }
            else if(otherPlayerController.currentAttack == Attack.Heavy)
            {
                StartCoroutine("HitByHeavy");
            }
            else
            {
                StartCoroutine("HitByNone");
            }

        }

    }
    */
    IEnumerator Die()
    {
        animationManager.Die();
        audioMan.PlaySound(AudioManager.SFX.FinalHit);
        actionable = false;
        gameController.RoundEnd(!P1);
        yield return null;
    }
    IEnumerator HitByLight()
    {
        audioMan.PlaySound(AudioManager.SFX.LightHit);
        animationManager.StartHitstun();
        inHitstun = true;
        actionable = false;
        health -= 1;
        healthDisplay.value = (float)health / MAX_HEALTH;

        if (health <= 0)
        {
            StartCoroutine("Die");
            yield return null;
        }

        rb2d.AddForce(new Vector2(-LIGHT_ATTACK_KNOCKBACK * direction, 0));
        spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);
        spr.color = new Color(200, 200, 200);

        animationManager.EndHitstun();
        inHitstun = false;
        actionable = true;
        yield return null;
    }

    IEnumerator HitByHeavy()
    {
        audioMan.PlaySound(AudioManager.SFX.HeavyHit);
        animationManager.StartHitstun();
        inHitstun = true;
        actionable = false;
        health -= 2;
        healthDisplay.value = (float)health / MAX_HEALTH;

        if (health <= 0)
        {
            StartCoroutine("Die");
            yield return null;
        }

        rb2d.AddForce(new Vector2(-HEAVY_ATTACK_KNOCKBACK * direction, 0));
        spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(HEAVY_ATTACK_STUN);
        spr.color = new Color(200, 200, 200);

        animationManager.EndHitstun();
        inHitstun = false;
        actionable = true;
        yield return null;
    }
    IEnumerator HitByNone()
    {
        inHitstun = true;
        actionable = false;
        health -= 0;
        rb2d.AddForce(new Vector2(-NONE_ATTACK_KNOCKBACK * direction, 0));
        spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);
        spr.color = new Color(200, 200, 200);

        inHitstun = false;
        actionable = true;
        yield return null;
    }


}
