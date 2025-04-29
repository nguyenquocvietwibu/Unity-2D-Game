using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update

    public Rigidbody2D rb2D;
    public float moveSpeed;
    public float jumpForce;
    public BoxCollider2D boxCollider2D;

    public LayerMask mask;

    public bool isGrounded;
    public bool isJumping;
    public bool canDoubleJump;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        moveSpeed = 7f;
        jumpForce = 20f;
    }

    // Update is called once per frame
    void Update()
    {

        rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2D.velocity.y);
        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, mask))
        {
            isGrounded = true;
            canDoubleJump = true;
            isJumping = false;
        }
        else
        {
            isGrounded = false;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            isJumping = true;
        }
        if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce / 1.5f);
            canDoubleJump = false;
            isJumping = true;
        }
    }
}

