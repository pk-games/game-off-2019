using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float characterSpeed = 5.0f;
    private float jumpHeight = 250.0f;
    private bool isGrounded = false;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        rigidBody.velocity = new Vector2(moveX * characterSpeed, rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {// Allow the player to jump if he is grounded
            rigidBody.AddForce(Vector2.up * jumpHeight);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        { // If Player is touching the ground, then set grounded to true
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    { // if Player is not touching is the ground, then set grounded to false
        if (collision.gameObject.name == "Ground")
        {
            isGrounded = false;
        }
    }

}
