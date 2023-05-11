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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightAttack()
    {
        swordHitBox.attackType = Hitbox.AttackType.Light;
        animator.ResetTrigger("LightAttack");
        animator.SetTrigger("LightAttack");
    }

    public void HeavyAttack()
    {
        swordHitBox.attackType = Hitbox.AttackType.Heavy;
        animator.ResetTrigger("HeavyAttack");
        animator.SetTrigger("HeavyAttack");
    }

    public void HeavyAttackLunge()
    {
        movementScript.Lunge();
    }

    public void StartHitstun()
    {
        animator.SetBool("Hitstun", true);
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
        StartHitstun();
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
