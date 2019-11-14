using UnityEngine;

public class Spike : MonoBehaviour
{
    private Vector2 RespawnPoint;

    private void Awake()
    {
        RespawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = RespawnPoint;
        }
    }
}
