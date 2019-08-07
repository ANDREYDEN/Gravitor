using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    private static CameraControl instance;
    private GameObject Player;
    public GameObject Background;
    public float cameraSpeed = 0.2f;

    public static CameraControl Instance { get => instance; set => instance = value; }

    //public float maxDistance;           // max distance between the camera and the player

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Background);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnFullLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnFullLoad;
    }

    private void OnFullLoad(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.State == 1)
        {
            Player = GameObject.FindWithTag("Player");
        }
    }
    
    void FixedUpdate()
    {
        if (GameManager.Instance.State == 1) {
            FollowObject(Player);
        }
        Background.transform.position = new Vector3(transform.position.x, transform.position.y, 1);                 // make background follow the camera
    }

    void FollowObject(GameObject obj)
    {
        // focus the camera on the object
        Vector3 target = new Vector3(Player.transform.position.x, Player.transform.position.y, -1);     // make sure z is the same as the camera's
        SmoothFollow(target);
    }

    void SmoothFollow(Vector3 targetPosition)
    {
        Vector3 refVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref refVelocity, cameraSpeed);
    }
}
