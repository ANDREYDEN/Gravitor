using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 0.2f;
    //public float maxDistance;           // max distance between the camera and the player

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // focus the camera on the player
        Vector3 target = new Vector3(player.transform.position.x, player.transform.position.y, -1);     // make sure z is the same with the camera
        SmoothFollow(target);

        // move the background with the camera
        GameObject background = GameObject.Find("BackGround");
        background.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    void SmoothFollow(Vector3 targetPosition)
    {
        Vector3 refVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, cameraSpeed);
    }
}
