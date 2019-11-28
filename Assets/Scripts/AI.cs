using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{

    private Transform target;
    private Animator animator;
    private float speed = 2.5f;
    private bool ChasePlayer = false;
    private SpriteRenderer spr;
    private Rigidbody2D rb;



    void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = gameObject.GetComponentInParent<Animator>();
        spr = GetComponentInParent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();

    }

    private void Update()
    {
        if(Exit.levelComplete)
        {
            animator.SetBool("Running", false);
        }
        else if(Player.isDead)
        {
            animator.SetBool("Running", false);

        }
        else if (ChasePlayer)
        {
            spr.flipX = (target.position.x < transform.position.x);
            if (spr.flipX)
            {
                GetComponentInParent<Anomaly>().velocity.x -= speed;
            }
            else
            {
                GetComponentInParent<Anomaly>().velocity.x += speed;
            }


            animator.SetBool("Running", true);
        } else {
            animator.SetBool("Running", false);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ChasePlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ChasePlayer = false;
        }
    }
}
 