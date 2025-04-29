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

    public RaycastHit2D hit2D;

    public bool canJump;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        moveSpeed = 5f;
        jumpForce = 10f;
    }

    // Update is called once per frame
    void Update()
    {

        rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, rb2D.velocity.y);
        if (Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, mask))
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
        }
    }
}

