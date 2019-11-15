using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Clone : MonoBehaviour
{
    Controller2D controller;
    Vector3 velocity;

    private readonly float gravity = Player.gravity;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
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
