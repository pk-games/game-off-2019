using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject trigger;
    private SpriteRenderer spriteRenderer;
    private bool isInRange;
    private bool isActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isInRange)
        {
            isActive = !isActive;
            spriteRenderer.flipX = isActive;

            // TODO: Call method on trigger class
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isInRange = false;
        }
    }
}
