using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterController : MonoBehaviour
{
    int bgNum;
    bool left;
    SpriteRenderer spr;

    public void Setup(int bg, bool _left)
    {
        bgNum = bg;
        left = _left;
    }
    // Start is called before the first frame update
    void Start()
    {
        spr = gameObject.GetComponent<SpriteRenderer>();
        if(bgNum == 0)
        {
            transform.position = new Vector3(-19.25f, 1.25f, 0f);
        }
        else if(bgNum == 1)
        {
            spr.enabled = false;
            transform.position = new Vector3(-22f, 0.69f, 0f);
        }

        if (!left)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, 0f);
            spr.flipX = true;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
