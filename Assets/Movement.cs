using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{

    GameObject sword;
    Rigidbody2D rb2d;
    TMP_Text healthDisplay;
    Movement otherMoveScript;
    SpriteRenderer spr;

    public bool P1;
    public int health;


    const float MOVE_SPEED = 5f;

    const float NONE_ATTACK_KNOCKBACK = 30f;

    const float LIGHT_ATTACK_STARTUP = 0.1f;
    const float LIGHT_ATTACK_ACTIVE = 0.3f;
    const float LIGHT_ATTACK_RECOVERY = 0.1f;
    const float LIGHT_ATTACK_DISTANCE = 0.6f;
    const float LIGHT_ATTACK_STUN = 0.2f;
    const float LIGHT_ATTACK_KNOCKBACK = 100f;

    const float HEAVY_ATTACK_STARTUP = 0.3f;
    const float HEAVY_ATTACK_ACTIVE = 0.5f;
    const float HEAVY_ATTACK_RECOVERY = 0.5f;
    const float HEAVY_ATTACK_DISTANCE = 0.9f;
    const float HEAVY_ATTACK_STUN = 0.4f;
    const float HEAVY_ATTACK_KNOCKBACK = 200f;

    public enum Attack
    {
        None, Light, Heavy
    }
    public Attack currentAttack;


    bool actionable = true;
    int direction;
    
    
    


    // Start is called before the first frame update
    void Start()
    {
        currentAttack = Attack.None;
        health = 8;
        
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        spr = gameObject.GetComponent<SpriteRenderer>();

        if (P1) 
        {
            healthDisplay = GameObject.Find("/Canvas/P1Health").GetComponent<TMP_Text>();
            sword = GameObject.Find("/Player1/P1Sword");
            direction = 1;
            otherMoveScript = GameObject.Find("Player2").GetComponent<Movement>();
            
        } 
        else 
        {
            healthDisplay = GameObject.Find("/Canvas/P2Health").GetComponent<TMP_Text>();
            sword = GameObject.Find("/Player2/P2Sword");
            direction = -1;
            otherMoveScript = GameObject.Find("Player1").GetComponent<Movement>();
        }
        healthDisplay.text = "Health: " + health.ToString();
    }
    //move forward
    //move backward
    //light attack
    //heavy attack
    //sheild

    // Update is called once per frame
    void Update()
    {
        

        if(actionable)
        {

            int lr = P1 ? (int)Input.GetAxisRaw("HorizontalP1") : (int)Input.GetAxisRaw("HorizontalP2");
            

            if (lr == -1)
            {
                gameObject.transform.Translate(new Vector2(-MOVE_SPEED, 0) * Time.deltaTime);
            }
            else if (lr == 1)
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

    IEnumerator LightAttack()
    {
        currentAttack = Attack.Light;
        actionable = false;
        yield return new WaitForSeconds(LIGHT_ATTACK_STARTUP);
        sword.transform.Translate(new Vector2(LIGHT_ATTACK_DISTANCE * direction, 0)); //stab extend

        yield return new WaitForSeconds(LIGHT_ATTACK_ACTIVE);
        sword.transform.Translate(new Vector2(-LIGHT_ATTACK_DISTANCE * direction, 0));

        yield return new WaitForSeconds(LIGHT_ATTACK_RECOVERY);

        currentAttack = Attack.None;
        actionable = true;

        yield return null;
    }

    IEnumerator HeavyAttack()
    {
        currentAttack = Attack.Heavy;
        actionable = false;
        spr.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(HEAVY_ATTACK_STARTUP);
        spr.color = new Color(200, 200, 200);
        sword.transform.Translate(new Vector2(HEAVY_ATTACK_DISTANCE * direction, 0)); //stab extend

        yield return new WaitForSeconds(HEAVY_ATTACK_ACTIVE);
        sword.transform.Translate(new Vector2(-HEAVY_ATTACK_DISTANCE * direction, 0));

        yield return new WaitForSeconds(HEAVY_ATTACK_RECOVERY * direction);

        
        currentAttack = Attack.None;
        actionable = true;

        yield return null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(P1 && col.name == "P2Sword" || !P1 && col.name == "P1Sword") 
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

    IEnumerator HitByLight()
    {
        actionable = false;
        health -= 1;
        healthDisplay.text = "Health: " + health.ToString();
        rb2d.AddForce(new Vector2(-LIGHT_ATTACK_KNOCKBACK * direction, 0));

        
        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);

        actionable = true;
        yield return null;
    }

    IEnumerator HitByHeavy()
    {
        actionable = false;
        health -= 2;
        healthDisplay.text = "Health: " + health.ToString();
        rb2d.AddForce(new Vector2(-HEAVY_ATTACK_KNOCKBACK * direction, 0));


        yield return new WaitForSeconds(HEAVY_ATTACK_STUN);

        actionable = true;
        yield return null;
    }
    IEnumerator HitByNone()
    {
        actionable = false;
        health -= 1;
        rb2d.AddForce(new Vector2(-NONE_ATTACK_KNOCKBACK * direction, 0));


        yield return new WaitForSeconds(LIGHT_ATTACK_STUN);

        actionable = true;
        yield return null;
    }


}
