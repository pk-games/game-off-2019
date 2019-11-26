using UnityEngine;

public class Artifact : MonoBehaviour
{
    public GameObject[] triggers;

    private bool isInRange;

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isInRange)
        {
            ToggleTriggers();
            Destroy(this.gameObject);
            Player.canWarp = true;
        }
    }

    private void ToggleTriggers()
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].SetActive(!triggers[i].activeSelf);
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
