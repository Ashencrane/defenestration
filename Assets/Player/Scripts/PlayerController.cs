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
    GameController gameController;




    const int MAX_HEALTH = 8;
    const float MOVE_SPEED = 4.75f;

    const float STARTING_DISTANCE = 6f;

    const float NONE_ATTACK_KNOCKBACK = 30f;

    const float FREEZE_FRAME_TIMESCALE = 0.2f;


    const float LIGHT_ATTACK_STUN = 0.25f;
    const float LIGHT_ATTACK_KNOCKBACK = 5f;
    const float LIGHT_ATTACK_BLOCK_KNOCKBACK = 10f;
    const float LIGHT_ATTACK_FREEZE_TIME = 0.01f;

    const float HEAVY_ATTACK_STUN = 0.6f;
    const float HEAVY_ATTACK_KNOCKBACK = 10f;
    const float HEAVY_ATTACK_BLOCK_KNOCKBACK = 20f;
    const float HEAVY_ATTACK_FREEZE_TIME = 0.02f;

    const float FORWARDASH_SPEED = 10f;
    const float FORWARDASH_ACTIVE = 0.35f;
    const float FORWARDASH_RECOVERY = 0.08f;

    const float BACKDASH_SPEED = 10f;
    const float BACKDASH_ACTIVE = 0.35f;
    const float BACKDASH_RECOVERY = 0.08f;

    const float CLASH_WINDOW = 0.02f;
    const float CLASH_STUN = 0.25f;
    const float CLASH_KNOCKBACK = 5f;
    const float CLASH_FREEZE_TIME = 0.001f;

    const float DEATH_FREEZE_TIME = 2.5f;

    const float LUNGE_FORCE = 10f;

    const float PARRY_WINDOW = 0.1f;
    const float PARRY_FREEZE_TIME = 0.04f;
    const float PARRY_DURATION = 0.4f;
    const float PARRY_COOLDOWN = 0.25f;
    

    public enum Attack
    {
        None, Light, Heavy
    }
    public Attack currentAttack;


    public bool P1;
    public int health;
    public bool actionable = true;
    public bool movable = true;
    bool atCameraEdge;
    bool inHitstun = false;
    int direction;
    public bool isHit = false;
    public bool isDefenestratable = false;
    public bool isBlocking = false;
    public bool isParrying = false;
    public bool isDead = false;
    public bool isInvuln = false;


    Color idleColor = new Color(1.0f, 1.0f, 1.0f);
    float parryCooldownTimer;
    public bool parryingAttack = false;

    int heldleftright = 0;
    double forwarDashSec = 0;
    double backDashSec = 0;

    [SerializeField]
    public AnimationManager animationManager;
    [SerializeField]
    SpriteRenderer[] spriteArray;


    [SerializeField]
    private GameObject[] bloodFX; //0 = weak, 1=strong, 2=death

    [SerializeField]
    private GameObject glassFX;

    [SerializeField]
    private AudioManager audioMan;

    private void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        

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
    }

    //Maps player input to their abilities and movements
    void Update()
    {
        atCameraEdge = Vector3.Distance(transform.position, otherPlayerController.transform.position) > 17;
        int leftright = P1 ? (int)Input.GetAxisRaw("HorizontalP1") : (int)Input.GetAxisRaw("HorizontalP2");
        if (backDashSec > 0)
        {
            backDashSec -= Time.deltaTime;
        }
        if (forwarDashSec > 0)
        {
            forwarDashSec -= Time.deltaTime;
        }
        if (parryCooldownTimer > 0)
        {
            parryCooldownTimer -= Time.deltaTime;
        }

        if (actionable && !isDead && !otherPlayerController.isDead)
        {
            bool dashed = false;
            rb2d.velocity = new Vector2(0f, 0f);
            //temp fix for z3
            //rewrite this code later
            bool fdash = P1 ? Input.GetAxis("DashP1") == 1 && Input.GetAxis("HorizontalP1") == 1 : Input.GetAxis("DashP2") == 1 && Input.GetAxis("HorizontalP2") == -1;
            bool bdash = P1 ? Input.GetAxis("DashP1") == 1 && Input.GetAxis("HorizontalP1") == -1 : Input.GetAxis("DashP2") == 1 && Input.GetAxis("HorizontalP2") == 1;
            //backdash code

            if (Input.GetKeyDown(KeyCode.A) && P1 || Input.GetKeyDown(KeyCode.RightArrow) && !P1 || bdash) //scuffed but works
            {
                if (backDashSec > 0 && !atCameraEdge)
                {
                    StartCoroutine("Backdash");
                    backDashSec = 0;
                    dashed = true;
                }
                else
                {
                    backDashSec = 0.25f;
                }
            }
            
            //forwardash code
            else if (Input.GetKeyDown(KeyCode.D) && P1 || Input.GetKeyDown(KeyCode.LeftArrow) && !P1 || fdash)//scuffed but works
            {
                if (forwarDashSec > 0)
                {
                    StartCoroutine("Forwardash");
                    forwarDashSec = 0;
                    dashed = true;
                }
                else
                {
                    forwarDashSec = 0.25f;
                }
            }
            if (!dashed)
            {

                if (P1 ? leftright == -1 : leftright == 1)
                {
                    isBlocking = true;
                    isParrying = false;
                }
                else if (P1 ? leftright == 1 && heldleftright != 1 : leftright == -1 && heldleftright != -1)
                {
                    Debug.Log("Parry");
                    isBlocking = false;
                    if (parryCooldownTimer <= 0)
                    {
                        StartCoroutine("ActivateParry");
                    }
                }
                else
                {
                    isBlocking = false;
                }

                if (leftright == -1 && !(P1 && atCameraEdge) && movable)
                {
                    gameObject.transform.Translate(new Vector2(-MOVE_SPEED, 0) * Time.deltaTime);
                }
                else if (leftright == 1 && !(!P1 && atCameraEdge) && movable)
                {
                    gameObject.transform.Translate(new Vector2(MOVE_SPEED, 0) * Time.deltaTime);
                }

                if (P1 ? Input.GetButtonDown("LightP1") : Input.GetButtonDown("LightP2"))
                {
                    StartCoroutine("LightAttack");
                }

                if (P1 ? Input.GetButtonDown("HeavyP1") : Input.GetButtonDown("HeavyP2"))
                {
                    StartCoroutine("HeavyAttack");
                }
            }
            
        }
        else
        {
            leftright = 0;
        }
        animationManager.direction = P1 ? leftright : -leftright;
        heldleftright = leftright;
    }
    public void NewRound()
    {
        animationManager.Reset();
        idleColor = new Color(1.0f, 1.0f, 1.0f);
        transform.position = new Vector3(STARTING_DISTANCE * -direction, -1.5f, 0);
        rb2d.velocity = Vector2.zero;
        health = MAX_HEALTH;
        currentAttack = Attack.None;
        healthDisplay.value = (float)health / MAX_HEALTH;
        actionable = false;
        inHitstun = false;
        isDead = false;
        isInvuln = false;
    }
    

    public void Lunge()
    {
        rb2d.velocity = new Vector2(LUNGE_FORCE * direction, 0);
    }

    public void EnterDefenestrationZone()
    {
        isDefenestratable = true;
        idleColor = new Color(1.0f, 1.0f, 0);
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }
    }

    public void ExitDefenestrationZone()
    {
        isDefenestratable = false;
        idleColor = new Color(1.0f, 1.0f, 1.0f);
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }
    }



    IEnumerator Backdash()
    {
        actionable = false;
        isBlocking = false;
        //spr.color = new Color(0.7f, 0.7f, 0.7f);
        rb2d.drag = 0;
        animationManager.DashBackward();
        rb2d.velocity = new Vector3(BACKDASH_SPEED * -direction, 0, 0);
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = new Color(0.7f, 0.8f, 1.0f);
        }

        yield return new WaitForSeconds(BACKDASH_ACTIVE);
        rb2d.velocity = new Vector3(0, 0, 0);
        rb2d.drag = 0.05f;
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }

        yield return new WaitForSeconds(BACKDASH_RECOVERY);
        animationManager.StopDash();
        
        //spr.color = new Color(200, 200, 200);
        actionable = true;
        rb2d.drag = 2.0f;
        

        yield return null;
    }

    IEnumerator Forwardash()
    {
        actionable = false;
        isBlocking = false;
        //spr.color = new Color(0.7f, 0.7f, 0.7f);
        rb2d.velocity = new Vector3(FORWARDASH_SPEED * direction, 0, 0);
        rb2d.drag = 0;
        animationManager.DashForward();
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = new Color(0.7f, 0.8f, 1.0f);
        }
        
        
        yield return new WaitForSeconds(FORWARDASH_ACTIVE);
        rb2d.drag = 0.05f;
        rb2d.velocity = new Vector3(0, 0, 0);
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }

        yield return new WaitForSeconds(FORWARDASH_RECOVERY);
        animationManager.StopDash();

        //spr.color = new Color(200, 200, 200);
        actionable = true;
        rb2d.drag = 2.0f;

        yield return null;
    }

    IEnumerator LightAttack()
    {
        isBlocking = false;
        audioMan.PlaySound(AudioManager.SFX.LightAtk);
        animationManager.LightAttack();
        //currentAttack = Attack.Light;

        yield return null;
        
    }

    IEnumerator HeavyAttack()
    {
        isBlocking = false;
        audioMan.PlaySound(AudioManager.SFX.HeavyAtk);
        animationManager.HeavyAttack();
        //currentAttack = Attack.Heavy;

        yield return null;
    }

    public void OnHit(Hitbox hitbox, Hitbox colHitbox)
    {
        if (isInvuln)
        {
            return;
        }
        isHit = true;
        //Debug.Log("collide");
        if (colHitbox.GetBoxType() == Hitbox.BoxType.Hit && !inHitstun && colHitbox.isActive && !parryingAttack) 
        {
            if (isParrying)
            {
                StartCoroutine("Parry");
            }
            else if (currentAttack != Attack.None && Mathf.Abs(animationManager.frameTimeElapsed - otherPlayerController.animationManager.frameTimeElapsed) < CLASH_WINDOW)
            {
                StartCoroutine("Clash");
                otherPlayerController.StartClash();
            }
            else if (currentAttack == Attack.None || animationManager.frameTimeElapsed < otherPlayerController.animationManager.frameTimeElapsed || otherPlayerController.parryingAttack)
            {
                //Debug.Log("hit");
                if (isDefenestratable)
                {
                    health = 0;
                    StartCoroutine("HitByHeavy");
                }
                else if (colHitbox.attackType == Attack.Light)
                {
                    //UnityEngine.Debug.Log("light");
                    if (isBlocking)
                    {
                        StartCoroutine("BlockLight");
                    }
                    else
                    {
                        StartCoroutine("HitByLight");
                    }
                }
                else if (colHitbox.attackType == Attack.Heavy)
                {
                    //UnityEngine.Debug.Log("heavy");
                    if (isBlocking)
                    {
                        StartCoroutine("BlockHeavy");
                    }
                    else
                    {
                        StartCoroutine("HitByHeavy");
                    }
                }
                else
                {
                    //UnityEngine.Debug.Log("none");
                    StartCoroutine("HitByNone");
                }
            }
        }
    }

    public void StartClash()
    {
        //inHitstun = true;
        audioMan.PlaySound(AudioManager.SFX.Clash);
        StartCoroutine("Clash");
    }

    IEnumerator Die()
    {
        isDead = true;
        actionable = false;
        otherPlayerController.actionable = false;
        audioMan.PlaySound(AudioManager.SFX.FinalHit);
        GameObject bfx = Instantiate(bloodFX[2], new Vector3(transform.position.x + 1 * -direction, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -direction * 90, 0));
        yield return new WaitForSeconds(0.1f);
        //time freeze
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(0.005f);
        Time.timeScale = 0.6f;
        
        animationManager.Die();

        float KODELAY = 1.2f;

        yield return new WaitForSeconds(KODELAY);
        gameController.SetTextKO();
        yield return new WaitForSeconds(DEATH_FREEZE_TIME - KODELAY);
        Time.timeScale = 1;
        gameController.RoundEnd(!P1);
        yield return null;
    }

    IEnumerator Defenestrate()
    {
        // do the same thing as Die() but with a different animation, different UI text displayed
        animationManager.StartHitstun();
        animationManager.Hurt();
        inHitstun = true;
        actionable = false;
        healthDisplay.value = (float)health / MAX_HEALTH;
        GameObject bfx = Instantiate(bloodFX[1], new Vector3(transform.position.x + 1 * -direction, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -direction * 90, 0));
        isDead = true;
        actionable = false;
        otherPlayerController.actionable = false;
        audioMan.PlaySound(AudioManager.SFX.FinalHit);
        
        Time.timeScale = 0.75f;
        animationManager.Defenestrate();
        yield return new WaitForSeconds(0.55f);
        audioMan.PlaySound(AudioManager.SFX.GlassBreak);
        ShatterController sc = Instantiate(glassFX, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<ShatterController>();
        sc.Setup(gameController.currentBG, P1);

        yield return new WaitForSeconds(0.15f);
        transform.position = new Vector3(transform.position.x, -10, 0); //send below stage
        yield return new WaitForSeconds(1f);
        gameController.SetTextDefenestration();
        
        Time.timeScale = 1;
        yield return new WaitForSeconds(3f);
        sc.Destroy();
        gameController.RoundEnd(!P1);
        yield return null;
    }

    IEnumerator Clash()
    {
        inHitstun = true;
        animationManager.StartHitstun();
        animationManager.Hurt();
        actionable = false;
        rb2d.velocity = new Vector2(-CLASH_KNOCKBACK * direction, 0);
        yield return new WaitForSeconds(CLASH_STUN);

        animationManager.EndHitstun();
        actionable = true;
        inHitstun = false;
        yield return null;
    }
    IEnumerator ActivateParry()
    {
        isParrying = true;
        yield return new WaitForSeconds(PARRY_WINDOW);
        isParrying = false;
        parryCooldownTimer = PARRY_COOLDOWN;
        yield return null;
    }
    IEnumerator Parry()
    {
        audioMan.PlaySound(AudioManager.SFX.Parry);
        actionable = true;
        movable = false;
        animationManager.Parry();
        animationManager.StartParryDelay();
        parryingAttack = true;

        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = new Color(0.5f, 0.5f, 1.0f);
        }

        Time.timeScale = FREEZE_FRAME_TIMESCALE;
        yield return new WaitForSeconds(PARRY_FREEZE_TIME);
        Time.timeScale = 1;

        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }

        yield return new WaitForSeconds(PARRY_DURATION);

        animationManager.EndParryDelay();
        parryingAttack = false;
        movable = true;
        yield return null;
    }

    IEnumerator HitByLight()
    {
        
        audioMan.PlaySound(AudioManager.SFX.LightHit);
        animationManager.StartHitstun();
        animationManager.Hurt();
        inHitstun = true;
        actionable = false;
        health -= 1;
        healthDisplay.value = (float)health / MAX_HEALTH;
        
        if (health <= 0)
        {
            StartCoroutine("Die");
            yield return null;
        }
        else
        {
            GameObject bfx = Instantiate(bloodFX[0], new Vector3(transform.position.x + 1 * -direction, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -direction * 90, 0));


            foreach (SpriteRenderer spr in spriteArray)
            {
                spr.color = new Color(1.0f, 0, 0);
            }

            Time.timeScale = FREEZE_FRAME_TIMESCALE;
            yield return new WaitForSeconds(LIGHT_ATTACK_FREEZE_TIME);
            Time.timeScale = 1;
            foreach (SpriteRenderer spr in spriteArray)
            {
                spr.color = idleColor;
            }
            
            rb2d.velocity = new Vector2(-LIGHT_ATTACK_KNOCKBACK * direction, 0);
            //spr.color = new Color(255, 0, 0);

            yield return new WaitForSeconds(LIGHT_ATTACK_STUN);
            //spr.color = new Color(200, 200, 200);

            animationManager.EndHitstun();
            inHitstun = false;
            actionable = true;
            isHit = false;
        }
        yield return null;
    }

    IEnumerator HitByHeavy()
    {
        audioMan.PlaySound(AudioManager.SFX.HeavyHit);
        animationManager.StartHitstun();
        animationManager.Hurt();
        inHitstun = true;
        actionable = false;
        health -= 2;
        healthDisplay.value = (float)health / MAX_HEALTH;

        if (isDefenestratable)
        {
            StartCoroutine("Defenestrate");
            yield return null;
        }
        else if (health <= 0)
        {
            StartCoroutine("Die");
            yield return null;
        }
        else
        {
            GameObject bfx = Instantiate(bloodFX[1], new Vector3(transform.position.x + 1 * -direction, transform.position.y + 0.5f, transform.position.z), Quaternion.Euler(0, -direction * 90, 0));
            foreach (SpriteRenderer spr in spriteArray)
            {
                spr.color = new Color(255, 0, 0);
            }

            Time.timeScale = FREEZE_FRAME_TIMESCALE;
            yield return new WaitForSeconds(HEAVY_ATTACK_FREEZE_TIME);
            Time.timeScale = 1;
            foreach (SpriteRenderer spr in spriteArray)
            {
                spr.color = idleColor;
            }

            rb2d.velocity = new Vector2(-HEAVY_ATTACK_KNOCKBACK * direction, 0);
            //spr.color = new Color(255, 0, 0);

            yield return new WaitForSeconds(HEAVY_ATTACK_STUN);
            //spr.color = new Color(200, 200, 200);

            animationManager.EndHitstun();
            inHitstun = false;
            actionable = true;
            isHit = false;
        }

        yield return null;
    }
    IEnumerator HitByNone()
    {
        inHitstun = true;
        actionable = false;
        health -= 0;
        rb2d.AddForce(new Vector2(-NONE_ATTACK_KNOCKBACK * direction, 0));
        //spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);
        //spr.color = new Color(200, 200, 200);

        inHitstun = false;
        actionable = true;
        isHit = false;
        yield return null;
    }

    IEnumerator BlockLight()
    {
        audioMan.PlaySound(AudioManager.SFX.BlockLight);
        animationManager.StartHitstun();
        animationManager.Block();
        inHitstun = true;
        actionable = false;
        
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = new Color(0.5f, 0.5f, 1.0f);
        }

        Time.timeScale = FREEZE_FRAME_TIMESCALE;
        yield return new WaitForSeconds(LIGHT_ATTACK_FREEZE_TIME);
        Time.timeScale = 1;
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }
        
        rb2d.velocity = new Vector2(-LIGHT_ATTACK_BLOCK_KNOCKBACK * direction, 0);
        //spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);
        //spr.color = new Color(200, 200, 200);

        animationManager.EndHitstun();
        inHitstun = false;
        actionable = true;
        isHit = false;

        yield return null;

    }

    IEnumerator BlockHeavy()
    {
        audioMan.PlaySound(AudioManager.SFX.BlockHeavy);
        animationManager.StartHitstun();
        animationManager.Block();
        inHitstun = true;
        actionable = false;
        
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = new Color(0.5f, 0.5f, 1.0f);
        }

        Time.timeScale = FREEZE_FRAME_TIMESCALE;
        yield return new WaitForSeconds(HEAVY_ATTACK_FREEZE_TIME);
        Time.timeScale = 1;
        foreach (SpriteRenderer spr in spriteArray)
        {
            spr.color = idleColor;
        }
        
        rb2d.velocity = new Vector2(-HEAVY_ATTACK_BLOCK_KNOCKBACK * direction, 0);
        //spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(HEAVY_ATTACK_STUN);
        //spr.color = new Color(200, 200, 200);

        animationManager.EndHitstun();
        inHitstun = false;
        actionable = true;
        isHit = false;

        yield return null;

    }

}
