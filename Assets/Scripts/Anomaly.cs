using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Anomaly : MonoBehaviour
{
    Controller2D controller;

    public Vector2 velocity;
    public Animator animator;

    private float velocityXSmoothing;
    private readonly float gravity = Player.gravity;
    private readonly float accelerationTimeGrounded = Player.accelerationTimeGrounded;
    private readonly float accelerationTimeAirborne = Player.accelerationTimeAirborne;

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
        float accelerationTime = controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne;
        velocity.x = Mathf.SmoothDamp(velocity.x, 0, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag == "Snapshot")
        {
            Destroy(collision.gameObject);
        }
    }
}
