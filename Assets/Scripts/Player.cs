using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{
    public static float maxJumpHeight = 2.2f;
    public static float minJumpHeight = 1;
    public static float timeToJumpApex = 0.3f;
    public static float accelerationTimeAirborne = 0.2f;
    public static float accelerationTimeGrounded = 0.05f;
    public static float moveSpeed = 8;
    public static float gravity;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    private bool isDead;

    public GameObject snapshotPrefab;
    public GameObject anomalyPrefab;

    private Animator animator;
    private Vector3 velocity;
    private Controller2D controller;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        float targetVelocityX;
        float accelerationTime;

        if (!isDead)
        {
            HandleAnimation();

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
            if (Input.GetButtonDown("Fire1"))
            {
                HandleWarp();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                HandleSetSnapshot();
            }

            targetVelocityX = Input.GetAxisRaw("Horizontal") * moveSpeed;
            accelerationTime = controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne;
        }
        else
        {
            targetVelocityX = 0;
            accelerationTime = 0;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    
    }

    void HandleAnimation()
    {
        float directionX = Input.GetAxisRaw("Horizontal");
        if (directionX > 0)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("Running", true);
        }
        else if (directionX < 0)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
        if (!controller.collisions.below)
        {
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("Falling", false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            isDead = true;
            animator.SetTrigger("Dead");
            StartCoroutine(RestartScene());
        }
    }

    IEnumerator RestartScene()
    {
        // Wait a second
        yield return new WaitForSeconds(1);

        // Fade to black
        Initiate.Fade("", Color.black, 1);

        // Wait a second
        yield return new WaitForSeconds(1);

        // Restart current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleWarp()
    {
        GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
        if (snapshot)
        {
            // Create an anomaly
            GameObject anomaly = Instantiate(anomalyPrefab, transform.position, Quaternion.identity);

            // Make anomalyface the same way as the player
            anomaly.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipX;

            // Transfer player velocity to anomaly
            anomaly.GetComponent<Anomaly>().velocity = velocity;

            // If snapshot exists move player to it
            transform.position = snapshot.transform.position;

            // Reset velocity
            velocity = Vector3.zero;

            // Destroy the snapshot
            Destroy(snapshot);
        }
    }

    private void HandleSetSnapshot()
    {
        GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
        if (snapshot)
        {
            // If snapshot already exists move it to current position
            snapshot.transform.position = transform.position;

            // Make snapshot face the same way as the player
            snapshot.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipX;
        }
        else
        {
            // Otherwise create a new snapshot at current location
            GameObject newSnapshot = Instantiate(snapshotPrefab, transform.position, Quaternion.identity);

            // Make snapshot face the same way as the player
            newSnapshot.GetComponent<SpriteRenderer>().flipX = this.GetComponent<SpriteRenderer>().flipX;
        }
    }
}