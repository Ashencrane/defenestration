using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // This class manaages the animation of the player model rig.

    [SerializeField]
    Animator animator;
    [SerializeField]
    Hitbox swordHitBox;
    [SerializeField]
    PlayerController movementScript;

    public bool isSwordActive;
    public float frameTimeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.currentAttack != PlayerController.Attack.None)
        {
            frameTimeElapsed += Time.deltaTime;
        }
        else { frameTimeElapsed = 0; }
    }

    public void ResetTime()
    {
        frameTimeElapsed = 0;
    }

    public void LightAttack()
    {
        swordHitBox.attackType = PlayerController.Attack.Light;
        animator.ResetTrigger("LightAttack");
        animator.SetTrigger("LightAttack");
    }

    public void HeavyAttack()
    {
        swordHitBox.attackType = PlayerController.Attack.Heavy;
        animator.ResetTrigger("HeavyAttack");
        animator.SetTrigger("HeavyAttack");
    }

    public void HeavyAttackLunge()
    {
        movementScript.Lunge();
    }

    public void Hurt()
    {
        animator.ResetTrigger("Hurt");
        animator.SetTrigger("Hurt");
    }

    public void StartHitstun()
    {
        animator.SetBool("Hitstun", true);
        movementScript.currentAttack = PlayerController.Attack.None;
    }

    public void EndHitstun()
    {
        animator.SetBool("Hitstun", false);
    }

    public void Die()
    {
        animator.ResetTrigger("Die");
        animator.SetTrigger("Die");
        animator.SetBool("Dead", true);
    }

    public void Reset()
    {
        animator.SetBool("Dead", false);
        EndHitstun();
        MakeActionable();
    }

    public void Block()
    {

    }

    public void MakeUnactionable()
    {
        movementScript.actionable = false;
    }

    public void MakeActionable()
    {
        movementScript.actionable = true;
        movementScript.currentAttack = PlayerController.Attack.None;
    }
}
