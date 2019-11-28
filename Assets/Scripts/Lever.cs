using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour
{
    public GameObject[] triggers;
    public bool isTimed;
    public float timerSeconds;
    public AudioClip leverSound;
    public AudioClip timerSound;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private bool isInRange;
    private bool isUsable = true;
    private bool isActive;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (isTimed)
        {
            audioSource.clip = timerSound;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && isInRange && isUsable)
        {
            ToggleTriggers();

            if (isTimed)
            {
                audioSource.Play();
                isUsable = false;
                StartCoroutine(OnTimerEnd());
            }
        }
    }

    private void ToggleTriggers()
    {
        isActive = !isActive;
        spriteRenderer.flipX = isActive;
        audioSource.PlayOneShot(leverSound);

        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].SetActive(!triggers[i].activeSelf);
        }
    }

    IEnumerator OnTimerEnd()
    {
        yield return new WaitForSeconds(timerSeconds);
        isUsable = true;
        audioSource.Stop();
        ToggleTriggers();
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
