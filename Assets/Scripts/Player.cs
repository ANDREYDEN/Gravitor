using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 2f;
    public float jumpForce;
    public int crystals = 0;
    private float horizontalMovement;
    private float verticalMovement;

    private Animator walkAnimator;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        walkAnimator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * speed;
        verticalMovement = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Move();
        // flip the player if the gravity direction has changed
        renderer.flipY = rigidbody.gravityScale < 0;
    }

    private void Move()
    {
        ////////////////////////////////// HORIZONTAL //////////////////////////////////////

        horizontalMovement *= Time.fixedDeltaTime;

        // check if the horizontal axis movement is significant
        if (!Mathf.Approximately(horizontalMovement, 0))
        {
            // flip the player depending on the walking direction
            renderer.flipX = (horizontalMovement < 0);
        }

        // update the velocity
        //rigidbody.velocity = new Vector2(horizontalMovement, rigidbody.velocity.y);

        Vector3 targetVelocity = new Vector2(horizontalMovement, rigidbody.velocity.y);
        Vector3 refVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, 0.05f);

        // trigger walking animation if there's enough horizontal movement
        walkAnimator.SetBool("walking", !Mathf.Approximately(horizontalMovement, 0));

        ///////////////////////////////// VERTICAL ///////////////////////////////////
        
        // check if the vertical axis movement is significant
        if (!Mathf.Approximately(verticalMovement, 0))
        {
            // change gravity only if the player is stationary
            if (Mathf.Approximately(rigidbody.velocity.y, 0))
            {
                if (verticalMovement < 0 ^ rigidbody.gravityScale > 0)
                {
                    // change gravity corresponding to the axis direction
                    rigidbody.gravityScale *= -1;
                }
            }
        }

        ///////////////////////////////// JUMP ///////////////////////////////////
        
        // only jump if the player is not moving vertically
        if (Mathf.Approximately(rigidbody.velocity.y, 0))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (rigidbody.gravityScale > 0)
                {
                    rigidbody.AddForce(Vector3.up*jumpForce, ForceMode2D.Impulse);
                }
                else
                {
                    rigidbody.AddForce(Vector3.down*jumpForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check what did the player hit
        switch (collision.collider.name)
        {
            case "Spikes":
                Respawn();
                break;
            case "Portal":
                Win();
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        switch (collider.name)
        {
            case "Pickables":
                GameObject pickables = GameObject.Find("Pickables");
                Tilemap tilemap = pickables.GetComponent<Tilemap>();

                // destroy a crystal the player touched
                tilemap.SetTile(tilemap.WorldToCell(transform.position), null);
                crystals++;
                break;
        }
    }

    // reloads the scene after player death
    public void Respawn()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // disables the player and shows the winning banner
    public void Win()
    {
        gameObject.SetActive(false);
        UIController.ActivateWinBanner(true);
    }
}
