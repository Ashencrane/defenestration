using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // This class manaages the animation of the player model rig.

    [SerializeField]
    Animator animator;
    [SerializeField]
    BoxCollider2D swordHitBox;

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
        animator.ResetTrigger("LightAttack");
        animator.SetTrigger("LightAttack");
    }

    public void HeavyAttack()
    {

    }

    public void Block()
    {

    }
}
