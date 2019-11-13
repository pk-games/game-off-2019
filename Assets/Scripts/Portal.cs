using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector2 StartPoint;

    private void Start()
    {
        // get the start point positions of where the character would initially spawn.
        StartPoint = GameObject.Find("StartPoint").transform.position;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "Player")
        { // when the player makes contact with the portal, set the players position to the "start point" game object
            GameObject.Find("Player").transform.position = StartPoint;
        }
    }
}
