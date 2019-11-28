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
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Player>().canWarp = true;
            MusicManager.instance.PlayIntenseMusic();
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
