using System.Collections;
using UnityEngine;


public class EnablePrompt : MonoBehaviour
{
    public GameObject Text;
    private void Update()
    {
       
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Text.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Text.SetActive(false);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Text.SetActive(true);
        }
    }
}
