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
    public static float moveSpeed = 5;
    public static float gravity;
    public bool canWarp;

    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float velocityXSmoothing;
    public static bool isDead;

    public GameObject snapshotPrefab;
    public GameObject anomalyPrefab;
    public AudioClip warpSound;
    public AudioClip footStep1;
    public AudioClip footStep2;
    public AudioClip footStep3;
    public AudioClip jumpingSound;
    public AudioClip landingSound;
    public AudioClip dyingSound;
    public AudioClip snapshotSound;

    public RuntimeAnimatorController playerAnimationController;
    public RuntimeAnimatorController playerWithArtifactAnimationController;

    private Animator animator;
    private Vector2 velocity;
    private Controller2D controller;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    public GameObject flash;

    void Start()
    {
        flash.SetActive(false);
        animator = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Calculate gravity from max jump height & time to jump apex
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);

        // Calculate max jump velocity from gravity & time to jump apex
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        // Calculate minjump velocity from gravity & min jump height
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "Level1" || scene.name == "Level2")
        {
            canWarp = false;
            animator.runtimeAnimatorController = playerAnimationController;
        }
        else
        {
            canWarp = true;
            animator.runtimeAnimatorController = playerWithArtifactAnimationController;
        }

    }

    void Update()
    {
        if(Exit.levelComplete)
        {
            animator.SetBool("Running", false);
        }
        HandleAnimation();
        HandleSound();
        HandleMovement();
    }

    void HandleSound()
    {

        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            audioSource.PlayOneShot(jumpingSound, 0.5f);
        }

        GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
        if (Input.GetButtonDown("Fire1") && canWarp && snapshot)
        {
            audioSource.PlayOneShot(warpSound, 0.5f);
        }

        if (Input.GetButtonDown("Fire2") && canWarp)
        {
            audioSource.PlayOneShot(warpSound, 0.5f);
        }
    }

    void HandleMovement()
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
            GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
            if (Input.GetButtonDown("Fire1") && canWarp && snapshot)
            {
                StartCoroutine(HandleWarp());
            }
            if (Input.GetButtonDown("Fire2") && canWarp)
            {
                HandleSetSnapshot();
            }
            if (Input.GetButtonDown("Restart"))
            {
                StartCoroutine(RestartScene(0));
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
        if (canWarp)
        {
            animator.runtimeAnimatorController = playerWithArtifactAnimationController;
        }
        else
        {
            animator.runtimeAnimatorController = playerAnimationController;
        }
        if (isDead)
        {
            animator.SetBool("Dead", true);
        }
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
        GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
        if (Input.GetButtonDown("Fire1") && canWarp && snapshot)
        {
            StartCoroutine(HandleFlash());
            animator.SetBool("Warping", true);
        }
        else
        {
            animator.SetBool("Warping", false);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Deadly" || collision.gameObject.tag == "Anomaly")
        {
            isDead = true;

            // Can't live in HandleSound because it's played every frame
            audioSource.PlayOneShot(dyingSound);

            StartCoroutine(RestartScene(1));
        }
    }

    IEnumerator RestartScene(float delay)
    {
        // Wait a second
        yield return new WaitForSeconds(delay);

        // Fade to black
        Initiate.Fade("", Color.black, 1);

        // Wait a second
        yield return new WaitForSeconds(1);

        // Restart current scene
        Caching.ClearCache();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        isDead = false;
    }

    IEnumerator HandleWarp()
    {
        GameObject snapshot = GameObject.FindGameObjectWithTag("Snapshot");
        if (snapshot)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(0.55f);
            Time.timeScale = 1;
            animator.updateMode = AnimatorUpdateMode.Normal;

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

    IEnumerator HandleFlash()
    {
        yield return new WaitForSecondsRealtime(0.55f);
        flash.SetActive(true);
        yield return null;
        flash.SetActive(false);
    }

    /**
     * This is not being called in code. This is called in the animator
     * event trigger. To see, load up the "animation" window, select
     * "Player" and then select "Running". You will see little stems
     * which call these functions. They needed to be divided up
     * else it will play the entire duration of the footsteps.
     */
    void FootStep1 ()
    {
        audioSource.PlayOneShot(footStep1);
    }
    void FootStep2()
    {
        audioSource.PlayOneShot(footStep2);
    }
    void FootStep3()
    {
        audioSource.PlayOneShot(footStep3);
    }

}
