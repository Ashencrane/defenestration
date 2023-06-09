using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CharacterSelector;

public class AnimationManager : MonoBehaviour
{
    // This class manaages the animation of the player model rig.

    [SerializeField]
    Animator animator;
    [SerializeField]
    Hitbox swordHitBox;
    [SerializeField]
    PlayerController movementScript;
    [SerializeField]
    UnityEngine.U2D.Animation.SpriteLibraryAsset[] characterSpriteLibraries;
    [SerializeField]
    UnityEngine.U2D.Animation.SpriteLibrary spriteLibrary;

    public bool isSwordActive;
    public float frameTimeElapsed;
    public Character currentCharacter = Character.Julie;

    public int direction; // 1 = forward, 0 = idle, -1 = backward
    
    private readonly Dictionary<Character, int> characterIndices = new Dictionary<Character, int>()
    {
        { Character.Julie, 0 },
        { Character.PissanxBebe, 1 }
    };

    // Start is called before the first frame update
    void Start()
    {
        currentCharacter = CharacterSelector.PlayerCharacters[movementScript.P1 ? 0 : 1];
        spriteLibrary.spriteLibraryAsset = characterSpriteLibraries[characterIndices[currentCharacter]];
    }

    // Update is called once per frame
    void Update()
    {
        if (movementScript.currentAttack != PlayerController.Attack.None)
        {
            frameTimeElapsed += Time.deltaTime;
        }
        else { frameTimeElapsed = 0; }
        animator.SetInteger("Direction", direction);
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

    public void Defenestrate()
    {
        animator.ResetTrigger("Defenestrate");
        animator.SetTrigger("Defenestrate");
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
        animator.ResetTrigger("Block");
        animator.SetTrigger("Block");
    }

    public void Parry()
    {
        animator.ResetTrigger("Parry");
        animator.SetTrigger("Parry");
    }
    public void StartParryDelay()
    {
        animator.SetBool("IsParrying", true);
    }
    public void EndParryDelay()
    {
        animator.SetBool("IsParrying", false);
    }

    public void DashForward()
    {
        animator.ResetTrigger("DashForward");
        animator.SetTrigger("DashForward");
        animator.SetBool("InDash", true);
    }

    public void DashBackward()
    {
        animator.ResetTrigger("DashBackward");
        animator.SetTrigger("DashBackward");
        animator.SetBool("InDash", true);
    }

    public void StopDash()
    {
        animator.SetBool("InDash", false);
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
