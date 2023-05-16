using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum BoxType
    {
        Hit, Hurt, Stop
    }

    [SerializeField]
    BoxType boxType = BoxType.Hurt;
    [SerializeField]
    PlayerController parentPlayerController;
    [SerializeField]
    SpriteRenderer sprite;

    public bool P1;

    public PlayerController.Attack attackType;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        if (boxType == BoxType.Hit)
        {
            isActive = false;
        }
        else
        {
            isActive = true;
        }
        P1 = parentPlayerController.P1;
    }

    // Update is called once per frame
    void Update()
    {
        if (boxType == BoxType.Hit)
        {
            parentPlayerController.currentAttack = attackType;
            if (isActive)
            {
                sprite.color = Color.red;
            }
            else
            {
                sprite.color = Color.green;
            }
        }

    }

    public BoxType GetBoxType()
    { return boxType; }

    void OnTriggerStay2D(Collider2D col)
    {
        Hitbox colHitbox = col.GetComponent<Hitbox>();

        
        if (colHitbox != null && colHitbox.P1 != P1 && boxType == BoxType.Hurt)
        {
            parentPlayerController.isHit = true;
            parentPlayerController.OnHit(this, colHitbox);
        }
        
    }
}
