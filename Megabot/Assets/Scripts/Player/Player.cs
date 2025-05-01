using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public BoxCollider2D boxCollider2D;

    public PlayerStateMachine stateMachine;

    public PlayerStates currentState;

    public CompositeCollider2D oneWayGroundCollider2D;

    public bool isFacingLeft;
    public bool isDamaged;
    public bool isGrounded;
    public bool isInOneWayGrounded;
    public bool isOnOneWayGrounded;

    public bool canDamaged;
    public bool canMove;
    public bool canJump;
    public bool canDoubleJump;

    public LayerMask enemyMask;


    public LayerMask groundMask;
    public LayerMask oneWayGroundMask;

    public Coroutine coroutine;
    public Stats stats;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.flipX)
        {
            isFacingLeft = true;
        }
        else
        {
            isFacingLeft = false;
        }

        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        stateMachine = new PlayerStateMachine(this);
        stateMachine.InitializeState(stateMachine.idleState);

        canDamaged = true;
        canMove = true;
        canJump = true;
        canDoubleJump = true;
        stats = Instantiate(stats);
    }

    private void Start()
    {

    }

    private void Update()
    {
        FacingCheck();
        if (isDamaged)
        {
            stateMachine.ChangeState(stateMachine.hitState);
        }
        if (canMove)
        {
            rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * stats.moveSpeed, rb2D.velocity.y);
        }
        //if (!isGrounded)
        //{
        //    CastHitBox();
        //}
        stateMachine.UpdateState();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            CastHitBox();
            CastHurtBox();
        }
    }

    public void Damage(float damage)
    {
        if (canDamaged)
        {
            isDamaged = true;
            stats.health -= damage;
            coroutine = StartCoroutine(BecomeInvincibility(1.75f));
        }
    }

    public void RemoveStunAndEnterIdle()
    {
        isDamaged = false;
        stateMachine.ChangeState(stateMachine.idleState);
    }

    public IEnumerator BecomeInvincibility(float invincibilityTime)
    {
        float blinkDuration = 0.05f;
        Color defaultColor = spriteRenderer.color;
        Color opacityColor = defaultColor;
        opacityColor.a = 0.4f;
        canDamaged = false;
        while (invincibilityTime >= 0)
        {
            spriteRenderer.color = opacityColor;
            yield return new WaitForSeconds(blinkDuration);
            invincibilityTime -= blinkDuration;
            spriteRenderer.color = defaultColor;
            yield return new WaitForSeconds(blinkDuration);
            invincibilityTime -= blinkDuration;
        }
        canDamaged = true;
    }

    public void GroundCheck()
    {
        if (!isInOneWayGrounded)
        {
            RaycastHit2D hit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.025f, groundMask);
            if (hit2D.collider != null)
            {
                isGrounded = true;
                canDoubleJump = true;
            }
            else
            {
                isGrounded = false;
            }
            hit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.025f, oneWayGroundMask);
            if (hit2D.collider != null)
            {
                isOnOneWayGrounded = true;
                canDoubleJump = true;
            }
            else
            {
                isOnOneWayGrounded = false;
            }

        }
    }

    public void InOneWayGroundCheck()
    {
        RaycastHit2D hit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.zero, 0f, oneWayGroundMask);
        if (hit2D.collider != null)
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

    public void FacingCheck()
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

    public IEnumerator DisableCollision(float disableTime)
    {
        Physics2D.IgnoreCollision(boxCollider2D, oneWayGroundCollider2D, true);
        isInOneWayGrounded = true;
        yield return new WaitForSeconds(disableTime);
        Physics2D.IgnoreCollision(boxCollider2D, oneWayGroundCollider2D, false);
    }

    public void CastHitBox()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.05f, enemyMask);
        if (raycastHit2D.collider != null)
        {
            IDamage damageEnemy = raycastHit2D.collider.GetComponent<IDamage>();
            if (damageEnemy != null)
            {
                damageEnemy.Damage(1f);
                stateMachine.ChangeState(stateMachine.jumpState);
            }
        }
    }

    public void CastHurtBox()
    {
        Vector2[] directionsVector2Array = new Vector2[]
        {
            Vector2.up,
            Vector2.left,
            Vector2.right
        };

        foreach (Vector2 directionVector2 in directionsVector2Array)
        {
            RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, directionVector2, 0.05f, enemyMask);
            if (raycastHit2D.collider != null)
            {
                Damage(1f);
                break;
            }
        }
        
    }
}

