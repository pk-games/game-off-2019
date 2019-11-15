using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{
    public float maxJumpHeight = 2.2f;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.3f;
    public float accelerationTimeAirborne = 0.2f;
    public float accelerationTimeGrounded = 0.05f;
    public float moveSpeed = 8;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    private float gravity;

    private Vector2 respawnPoint;

    Vector3 velocity;
    Controller2D controller;

    private void Awake()
    {
        // Set spawn point
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void Start()
    {
        controller = GetComponent<Controller2D>();

        // Calculate gravity from max jump height & time to jump apex
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        // Calculate max jump velocity from gravity & time to jump apex
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // Calculate minjump velocity from gravity & min jump height
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void Update()
    {
        // Stop vertical velocity if we're touching the floor or roof
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        // Jump at max velocity if we're grounded
        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;
        }
        // On jump release set velocity to min jump velocity if it's slower (quickly released)
        if (Input.GetButtonUp("Jump") && velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }

        float targetVelocityX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float accelerationTime = controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void Respawn()
    {
        // Move to respawn point
        transform.position = respawnPoint;

        // Reset velocity
        velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            Respawn();
        }
    }
}