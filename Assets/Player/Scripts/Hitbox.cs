using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public enum BoxType
    {
        Hit, Hurt, Stop
    }
    public enum AttackType
    {
        Light, Heavy, None
    }

    [SerializeField]
    BoxType boxType = BoxType.Hurt;
    [SerializeField]
    Movement parentMovement;

    public bool P1;

    public AttackType attackType;

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
        P1 = parentMovement.P1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BoxType GetBoxType()
    { return boxType; }

    void OnTriggerEnter2D(Collider2D col)
    {
        UnityEngine.Debug.Log(col.gameObject.name);
        Hitbox colHitbox = col.GetComponent<Hitbox>();
        
        if (colHitbox != null && colHitbox.P1 != P1 && boxType == BoxType.Hurt)
        {
            UnityEngine.Debug.Log("hit");
            parentMovement.OnHit(this, colHitbox);
        }
        
    }
}
