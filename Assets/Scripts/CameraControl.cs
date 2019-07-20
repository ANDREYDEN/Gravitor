using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject player;
    public float lerpScale = 2f;

    // Start is called before the first frame update
    void Start()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        // focus the camera on the player
        Vector3 targetPosition = player.transform.position;
        Vector3 refVelocity = Vector3.zero;
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, 0.1f);
        transform.position = new Vector3(newPosition.x, newPosition.y, -1);
        //transform.position = new Vector3(playerPosition.x, playerPosition.y, -1);


        // move the background with the camera
        GameObject background = GameObject.Find("BackGround");
        background.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
