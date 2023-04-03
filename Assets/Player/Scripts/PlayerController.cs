using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    float playerSpeed;

    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        rb.position = horizontalInput == 0 ? rb.position : (horizontalInput > 0 ? rb.position + (Vector2.right * playerSpeed * Time.deltaTime) : rb.position - (Vector2.right * playerSpeed * Time.deltaTime));
    }
}
