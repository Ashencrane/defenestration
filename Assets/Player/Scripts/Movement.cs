using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Movement : MonoBehaviour
{

    //GameObject sword;
    Rigidbody2D rb2d;
    Slider healthDisplay;
    Movement otherMoveScript;
    SpriteRenderer spr;
    GameController gameController;
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

    const float BACKDASH_FORCE = 320f;
    const float BACKDASH_RECOVERY = 0.5f;

    public enum Attack
    {
        None, Light, Heavy
    }
    public Attack currentAttack;


    bool actionable = true;
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

        if (P1) 
        {
            healthDisplay = GameObject.Find("/Canvas/P1Health/P1HealthOutline").GetComponent<Slider>();
            //sword = GameObject.Find("/Player1/P1Sword");
            direction = 1;
            otherMoveScript = GameObject.Find("Player2").GetComponent<Movement>();
            
        } 
        else 
        {
            healthDisplay = GameObject.Find("/Canvas/P2Health/P2HealthOutline").GetComponent<Slider>();
            //sword = GameObject.Find("/Player2/P2Sword");
            direction = -1;
            otherMoveScript = GameObject.Find("Player1").GetComponent<Movement>();
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
        transform.position = new Vector3(STARTING_DISTANCE * -direction, 0, 0);
        health = MAX_HEALTH;
        currentAttack = Attack.None;
    }


    IEnumerator Backdash()
    {
        actionable = false;
        rb2d.AddForce(new Vector2(-BACKDASH_FORCE * direction, 0));
        spr.color = new Color(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(BACKDASH_RECOVERY);
        spr.color = new Color(200, 200, 200);
        actionable = true;

        yield return null;
    }

    IEnumerator LightAttack()
    {
        animationManager.LightAttack();
        currentAttack = Attack.Light;
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

        yield return null;
    }

    IEnumerator HeavyAttack()
    {
        currentAttack = Attack.Heavy;
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

        yield return null;
    }

    public void OnHit(Hitbox hitbox, Hitbox colHitbox)
    {
        UnityEngine.Debug.Log("2");
        if (colHitbox.GetBoxType() == Hitbox.BoxType.Hit && !inHitstun)
        {
            if (colHitbox.attackType == Hitbox.AttackType.Light)
            {
                StartCoroutine("HitByLight");
            }
            else if (colHitbox.attackType == Hitbox.AttackType.Heavy)
            {
                StartCoroutine("HitByHeavy");
            }
            else
            {
                StartCoroutine("HitByNone");
            }
        }
    }
    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        if(P1 && col.tag == "HitboxP2" || !P1 && col.tag == "HitboxP1") 
        {
            Debug.Log(otherMoveScript.currentAttack);
            if(otherMoveScript.currentAttack == Attack.Light)
            {
                StartCoroutine("HitByLight");
            }
            else if(otherMoveScript.currentAttack == Attack.Heavy)
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
        gameController.RoundEnd(!P1);
        yield return null;
    }
    IEnumerator HitByLight()
    {

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

        inHitstun = false;
        actionable = true;
        yield return null;
    }

    IEnumerator HitByHeavy()
    {

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
