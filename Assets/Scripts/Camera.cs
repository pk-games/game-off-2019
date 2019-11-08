using UnityEngine;

public class Camera : MonoBehaviour
{
    public float FollowSpeed = 2f;
    private Transform Target;
    private GameObject Player;

    private void Awake()
    { // Find the Player in the scene and assign to the target variable. this makes the camera focus on the player.
        Target = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = Target.position;
        newPosition.z = -5;
        transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
    }
}
