using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float jumpForce;

    public Rigidbody2D rb2D;
    public BoxCollider2D boxCollider2D;
    public CompositeCollider2D onWayPlatformdCollider2D;
    public Animator animator;


    public LayerMask jumpablePlatformsMask;
    public LayerMask oneWayPlatformMask;
    public LayerMask playerMask;
    public LayerMask enemyMask;

    public bool isGrounded;
    public bool isOneWayGrounded;
    public bool isAirborne;
    public bool isJumping;
    public bool canDoubleJump;
    public bool isRunning;
    public bool canCheckGround;

    public GameState gameState;

    public Vector2 movementVector2;
    public enum GameState
    {
        Start,
        Update,
        Exit
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        moveSpeed = 8f;
        jumpForce = 25f;

    }

    public void Anims()
    {
        animator.Play("Idle");
        animator.Play("Run");
        animator.Play("Jump");
        animator.Play("Fall");

        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpablePlatformsMask))
        {
            isGrounded = true;
            canDoubleJump = true;
            isJumping = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        if (isGrounded)
        {
            if (rb2D.velocity == Vector2.zero)
            {
                Idle();
            }

            if (rb2D.velocity.x != 0f)
            {
                Run();
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

        }
        else
        {
            if (Mathf.Approximately(rb2D.velocity.y, 0f) || rb2D.velocity.y <= 0f)
            {
                Debug.Log("Giống");
                Fall();
            }
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                DoubleJump();
            }
        }
    }

    IEnumerator DisableCollision()
    {
        Physics2D.IgnoreCollision(boxCollider2D, onWayPlatformdCollider2D, true);
        animator.Play("Fall");
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(boxCollider2D, onWayPlatformdCollider2D, false);
    }

    public void GroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpablePlatformsMask))
        {
            isGrounded = true;
            isAirborne = false;
            canDoubleJump = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
            isAirborne = true;
        }
    }

    public void Fall()
    {
        GroundCheck();
        animator.Play("Fall");
        if (isGrounded)
        {
            Idle();
        }
       
    }
    public void Move()
    {
        rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2D.velocity.y);
    }
    public void Jump()
    {
        animator.Play("Jump");
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        
        isAirborne = true;
        isJumping = true;
        isGrounded = false;
    }

    public void DoubleJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce / 1.5f);
        animator.Play("Double Jump");
        isAirborne = true;
        isJumping = true;
        isGrounded = false;
        canDoubleJump = false;
    }
  
    public void Idle()
    {
        GroundCheck();
        animator.Play("Idle");
    }

    public void Run()
    {
        GroundCheck();
        animator.Play("Run");
        
    }
}

