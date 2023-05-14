using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameController gc;
    private void Awake()
    {
        gc = GameObject.Find("GameController").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 1.3f, -10f);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position = new Vector3((gc.P1.transform.position.x + gc.P2.transform.position.x) / 2, 1.3f, -10f);
        if (transform.position.x < -9)
        {
            transform.position = new Vector3(-9, 1.3f, -10f);
        }
        if (transform.position.x > 9)
        {
            transform.position = new Vector3(9, 1.3f, -10f);
        }
    }
}
