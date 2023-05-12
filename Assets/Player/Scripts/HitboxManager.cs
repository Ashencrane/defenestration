using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxManager : MonoBehaviour
{
    [SerializeField]
    List<SpriteRenderer> hitboxSpriteRenderers;


    bool hitboxesToggled = false;
    // Start is called before the first frame update
    private void Start()
    {
        foreach(SpriteRenderer spr in hitboxSpriteRenderers)
        {
            spr.enabled = false;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (hitboxesToggled)
            {
                foreach (SpriteRenderer spr in hitboxSpriteRenderers)
                {
                    hitboxesToggled = false;
                    spr.enabled = false;
                }
            }
            else
            {
                foreach (SpriteRenderer spr in hitboxSpriteRenderers)
                {
                    hitboxesToggled = true;
                    spr.enabled = true;
                }
            }
        }
    }
}
