using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour
{
    private GameObject Door;
    private Transform Target;

    private void Start()
    {
        Target = GameObject.Find("Door").transform;
    }

    void OnTriggerEnter2D(Collider2D other)
    { // If player is pressing the button, then open the door
        if(other.gameObject.name=="Player")
        {
            StartCoroutine("OpenDoor");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        { //if player is off the button, stop opening the door
           StopCoroutine("OpenDoor");
        }
    }

    IEnumerator OpenDoor()
    {
        while(true)
        {
            Target.transform.Translate(0, Time.deltaTime, 0);
            yield return null;
        }
    }
}
