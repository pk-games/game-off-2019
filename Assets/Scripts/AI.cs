using System.Collections;
using UnityEngine;

public class AI : MonoBehaviour
{

    private Transform target;
    private Animator animator;
    private float speed = 2f;
    private bool ChasePlayer = false;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = gameObject.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if(ChasePlayer)
        {
            transform.parent.LookAt(target.position);
            transform.parent.Rotate(new Vector3(0, -90, 0), Space.Self);
            transform.parent.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            
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
 