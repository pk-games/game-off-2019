using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Clone : MonoBehaviour
{
    Controller2D controller;
    Vector3 velocity;

    private float maxJumpHeight = Player.maxJumpHeight;
    private float timeToJumpApex = Player.timeToJumpApex;
    private float gravity;

    void Start()
    {
        //animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        // Calculate gravity from max jump height & time to jump apex
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        // Calculate max jump velocity from gravity & time to jump apex
        //maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // Calculate minjump velocity from gravity & min jump height
        //minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        // Stop vertical velocity if we're touching the floor or roof
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            Destroy(this.gameObject);
        }
    }
}
