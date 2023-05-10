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
    Movement movementScript;

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
        movementScript.currentAttack = Movement.Attack.None;
    }
}
