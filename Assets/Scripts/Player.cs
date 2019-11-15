using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour
{
    public float maxJumpHeight = 2.2f;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.3f;
    public float accelerationTimeAirborne = 0.2f;
    public float accelerationTimeGrounded = 0.05f;
    public float moveSpeed = 8;

    public GameObject warpPointPrefab;
    public GameObject clonePrefab;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    private float gravity;
    private bool isDead = false;

    private Animator animator;

    Vector3 velocity;
    Controller2D controller;
    SpriteRenderer spriteRenderer;



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
        if (isDead) return;
        AnimationHandler();
        //spriteRenderer.flipX = isFacingRight;
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
        if (Input.GetButtonDown("Fire1"))
        {
            HandleWarp();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            HandleSetWarp();
        }

        float targetVelocityX = Input.GetAxisRaw("Horizontal") * moveSpeed;
        float accelerationTime = controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    
    }

    void AnimationHandler ()
    {
        if(Input.GetButton("isDead"))
        {
            animator.SetTrigger("isDead");
            isDead = true;
        }
        float directionX = Input.GetAxis("Horizontal");
        print(directionX);
        if(directionX>0)
        {
            spriteRenderer.flipX = false;
        }
        if(directionX<0)
        {
            spriteRenderer.flipX = true;
        }
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Jump", true);
        }
        else if (controller.collisions.below)
        {
            animator.SetBool("Jump", false);
        }
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly")
        {
            StartCoroutine(ReloadLevel());
        }
    }

    IEnumerator ReloadLevel()
    {
        Initiate.Fade("", UnityEngine.Color.black, 1.5f);
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandleWarp()
    {
        GameObject warpPoint = GameObject.FindGameObjectWithTag("WarpPoint");
        if (warpPoint)
        {
            // Create a clone
            Instantiate(clonePrefab, transform.position, Quaternion.identity);

            // If warp point exists move player to it
            transform.position = warpPoint.transform.position;

            // Reset velocity
            velocity = Vector3.zero;

            // Destroy warp point
            Destroy(warpPoint);
        }
    }

    private void HandleSetWarp()
    {
        GameObject warpPoint = GameObject.FindGameObjectWithTag("WarpPoint");
        if (warpPoint)
        {
            // If warp point already exists move it to current position
            warpPoint.transform.position = transform.position;
        }
        else
        {
            // Otherwise create a new warp point at current location
            Instantiate(warpPointPrefab, transform.position, Quaternion.identity);
        }
    }
}