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

    private Animator _animator;
    private SpriteRenderer _renderer;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

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
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
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
            _renderer.flipX = (horizontalInput < 0);                                 // flip the player depending on the walking direction
        }

        // update the velocity
        Vector3 targetVelocity = new Vector2(horizontalInput, _rigidbody.velocity.y);
        Vector3 refVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref refVelocity, 0.03f);

        ///////////////////////////////// VERTICAL ///////////////////////////////////
        
        if (!Mathf.Approximately(verticalInput, 0))                                 // check if the vertical axis movement is significant
        {
            if (Mathf.Approximately(_rigidbody.velocity.y, 0) && !CeilingCheck())    // change gravity only if the player is stationary and not touching the ceiling
            {
                if (verticalInput < 0 ^ _rigidbody.gravityScale > 0)
                {
                    _rigidbody.gravityScale *= -1;                                   // switch gravity *for the player*
                }
            }
        }

        ///////////////////////////////// JUMP ///////////////////////////////////
        
        if (Mathf.Approximately(_rigidbody.velocity.y, 0) && !CeilingCheck())        // only jump if the player is not moving vertically (and did not hit the ceiling)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Vector3 direction = (_rigidbody.gravityScale > 0) ? Vector3.up : Vector3.down;
                _rigidbody.AddForce(jumpForce * direction, ForceMode2D.Impulse);
            }
        }
    }

    /// <summary>
    /// Freezes/Unfreezes the player
    /// </summary>
    public void FreezeToggle()
    {
        _rigidbody.simulated = !_rigidbody.simulated;
    }

    /// <summary>
    /// Animates the character depending on the movement
    /// </summary>
    private void Animate()
    {
        _animator.SetBool("walking", !Mathf.Approximately(horizontalInput, 0));          // trigger walking animation if there's enough horizontal movement

        // TODO: jumping animation

        _renderer.flipY = _rigidbody.gravityScale < 0;                                    // flip the player if the gravity direction has changed
    }

    /// <summary>
    /// Check if there's ceiling above the character
    /// </summary>
    private bool CeilingCheck()
    {
        // find all colliders that are overlaping the circle at the head of the player. Search in 'Platforms' layer
        float direction = Mathf.Sign(_rigidbody.gravityScale);
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y + direction*_collider.size.y/2);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, _collider.size.x / 3, LayerMask.GetMask("Platforms"));
        return colliders.Length > 0;
    }
}
