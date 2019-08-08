using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private static Player instance;

    public float speed = 2f;
    public float jumpForce;
    private float horizontalInput;
    private float verticalInput;

    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidbody;
    private CapsuleCollider2D collider;

    public static Player Instance { get => instance; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal") * speed;
        verticalInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Move();
        Animate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.name)
        {
            case "Spikes":
                gameObject.SetActive(false);
                GameManager.Instance.Respawn(0, true);
                break;
            case "Portal":
                gameObject.SetActive(false);                    
                GameManager.Instance.LevelComplete();
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
                // **** PLAUSABLE BUG **** //
                tilemap.SetTile(tilemap.WorldToCell(transform.position), null);
                GameManager.Instance.IncreaseScore();
                break;
        }
    }

    private void Move()
    {
        ////////////////////////////////// HORIZONTAL //////////////////////////////////////

        horizontalInput *= Time.fixedDeltaTime;
        if (!Mathf.Approximately(horizontalInput, 0))                               // check if the horizontal axis movement is significant
        {
            renderer.flipX = (horizontalInput < 0);                                 // flip the player depending on the walking direction
        }

        // update the velocity
        Vector3 targetVelocity = new Vector2(horizontalInput, rigidbody.velocity.y);
        Vector3 refVelocity = Vector3.zero;
        rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity, targetVelocity, ref refVelocity, 0.03f);

        ///////////////////////////////// VERTICAL ///////////////////////////////////
        
        if (!Mathf.Approximately(verticalInput, 0))                                 // check if the vertical axis movement is significant
        {
            if (Mathf.Approximately(rigidbody.velocity.y, 0) && !CeilingCheck())    // change gravity only if the player is stationary and not touching the ceiling
            {
                if (verticalInput < 0 ^ rigidbody.gravityScale > 0)
                {
                    rigidbody.gravityScale *= -1;                                   // switch gravity *for the player*
                }
            }
        }

        ///////////////////////////////// JUMP ///////////////////////////////////
        
        if (Mathf.Approximately(rigidbody.velocity.y, 0) && !CeilingCheck())        // only jump if the player is not moving vertically (and did not hit the ceiling)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 direction = (rigidbody.gravityScale > 0) ? Vector3.up : Vector3.down;
                rigidbody.AddForce(jumpForce * direction, ForceMode2D.Impulse);
            }
        }
    }

    public void FreezeToggle()
    {
        rigidbody.simulated = !rigidbody.simulated;
    }

    /// <summary>
    /// Animates the character depending on the movement
    /// </summary>
    private void Animate()
    {
        animator.SetBool("walking", !Mathf.Approximately(horizontalInput, 0));          // trigger walking animation if there's enough horizontal movement

        // TODO: jumping animation

        renderer.flipY = rigidbody.gravityScale < 0;                                    // flip the player if the gravity direction has changed
    }

    /// <summary>
    /// Check if there's ceiling above the character
    /// </summary>
    private bool CeilingCheck()
    {
        // find all colliders that are overlaping the circle at the head of the player. Search in 'Platforms' layer
        float direction = Mathf.Sign(rigidbody.gravityScale);
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y + direction*collider.size.y/2);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, collider.size.x / 3, LayerMask.GetMask("Platforms"));
        return colliders.Length > 0;
    }
}
