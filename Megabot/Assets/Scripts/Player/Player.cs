using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    public float jumpForce;

    public Rigidbody2D rb2D;
    public BoxCollider2D boxCollider2D;
    public CompositeCollider2D onWayPlatformdCollider2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public LayerMask jumpablePlatformsMask;
    public LayerMask oneWayPlatformMask;
    public LayerMask playerMask;
    public LayerMask enemyMask;

    public bool isGrounded;
    public bool isOnOneWayGrounded;
    public bool isInOneWayGrounded;

    //public bool isAirborne;
    public bool isJumping;
    public bool isFalling;
    public bool canDoubleJump;
    public bool isRunning;
    public bool canCheckGround;

    public GameState gameState;

    public Vector2 movementVector2;

    public bool isFacingLeft;
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
        jumpForce = 21f;

        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (spriteRenderer.flipX)
        {
            case true:
                isFacingLeft = true;
                break;
            case false:
                isFacingLeft = false;
                break;
        }
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

        if (isGrounded)
        {
            if (rb2D.velocity == Vector2.zero)
            {
                Idle();
                GroundCheck();
            }

            if (rb2D.velocity.x != 0f)
            {
                Run();
                GroundCheck();
            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (isOnOneWayGrounded)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    StartCoroutine(DisableCollision());
                }
            }

        }
        else
        {

            if (isFalling)
            {
                GroundCheck();
            }
            if (Mathf.Approximately(rb2D.velocity.y, 0f) || rb2D.velocity.y <= 0f)
            {
                Fall();
            }
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                DoubleJump();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        InOneWayGroundCheck();
        Move();
        FlipXSpriteCheck();
        if (isGrounded)
        {

            if (rb2D.velocity == Vector2.zero)
            {
                Idle();
                GroundCheck();
            }
            
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                Run();
                GroundCheck();
            }
            if (isOnOneWayGrounded)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    Debug.Log("True");
                    StartCoroutine(DisableCollision());
                }
            }
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {    
            if (isFalling)
            {
                isJumping = false;
                GroundCheck();
            }
            if (rb2D.velocity.y <= 0f)
            {
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
        isInOneWayGrounded = true;
        Fall();
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(boxCollider2D, onWayPlatformdCollider2D, false);
    }

    public void GroundCheck()
    {
        if (!isJumping && !isInOneWayGrounded)
        {
            if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, jumpablePlatformsMask))
            {
                isGrounded = true;
                canDoubleJump = true;
                isJumping = false;
                isFalling = false;
            }
            else
            {
                isGrounded = false;

            }
            if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, oneWayPlatformMask))
            {
                isOnOneWayGrounded = true;

                canDoubleJump = true;
                isJumping = false;
            }
            else
            {
                isOnOneWayGrounded = false;

            }
        }
    }

    public void InOneWayGroundCheck()
    {
        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.zero, 0, oneWayPlatformMask))
        {
            isInOneWayGrounded = true;
            isGrounded = false;
            isOnOneWayGrounded = false;
        }
        else
        {
            isInOneWayGrounded = false;
        }
    }

    public void FlipXSpriteCheck()
    {
        if (isFacingLeft && Input.GetAxisRaw("Horizontal") > 0)
        {
            isFacingLeft = false;
            spriteRenderer.flipX = isFacingLeft;
        }
        else if (!isFacingLeft && Input.GetAxisRaw("Horizontal") < 0)
        {
            isFacingLeft = true;
            spriteRenderer.flipX = isFacingLeft;
        }
    }

    public void Fall()
    {
        isFalling = true;
        isJumping = false;
        isGrounded = false;
        isOnOneWayGrounded = false;
        GroundCheck();
        animator.Play("Fall");

    }
    public void Move()
    {
        rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2D.velocity.y);
    }
    public void Jump()
    {
        animator.Play("Jump");
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);

        isJumping = true;
        isGrounded = false;
    }

    public void DoubleJump()
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce / 1.5f);
        animator.Play("Double Jump");
        isJumping = true;
        isGrounded = false;
        isOnOneWayGrounded = false;
        canDoubleJump = false;
        GroundCheck();
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

