using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour , IDamage, IStats 
{
    public Rigidbody2D rb2D;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isFacingLeft;

    public Stats stats;

    public bool shouldAttack;

    public bool shouldTurnBack;

    public bool shouldRun;

    public bool isDamaged;

    public bool isDie;

    public LayerMask engageCheckMask;
    public LayerMask patrolCheckMask;


    private float engageCheckGizmosRayDistance;
    public float engageCheckRayDistance;
    public float patrolCheckGizmosRayDistance;
    public float patrolCheckRayDistance;

    public Coroutine coroutine;

    public Stats Stats { get => stats; set => stats = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;

        stats = Instantiate(stats);

        if (spriteRenderer.flipX)
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }

        engageCheckRayDistance = 20f;
        patrolCheckRayDistance = 2f;

        coroutine = StartCoroutine(RandomPatrolRun());
    }

    private void OnEnable()
    {
        isDie = false;
        isDamaged = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDamaged || !isDie)
        {

            CastEngagePlayerCheck();
            CastPatrolCheck();
            if (shouldAttack)
            {
                Attack();
            }
            else if (shouldTurnBack)
            {
                TurnBack();
            }
            else if (shouldRun)
            {
                Run();
            }
            else
            {
                Idle();
                FollowPlayer();
            }
        }
        if (transform.position.y <= -15f)
        {
            gameObject.SetActive(false);
        }
       
    }


    public void Idle()
    {
        animator.Play("Idle");
        rb2D.velocity = new Vector2(0f,0f);
    }

    public void Run()
    {
        animator.Play("Run");
        if (isFacingLeft)
        {
            rb2D.velocity = new Vector2(-stats.moveSpeed, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(stats.moveSpeed, rb2D.velocity.y);
        }
    }

        public void Attack()
    {
        rb2D.velocity = new Vector2(0f,0f);
        animator.Play("Run");
        if (isFacingLeft)
        {
            rb2D.velocity = new Vector2(-stats.moveSpeed * 2f, rb2D.velocity.y);
        }
        else
        {

            rb2D.velocity = new Vector2(stats.moveSpeed * 2f, rb2D.velocity.y);
        }
    }


    public void CastEngagePlayerCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxCollider2D.bounds.center, (isFacingLeft) ? Vector2.left : Vector2.right, engageCheckRayDistance, engageCheckMask);
        if (hit.collider != null )
        {
            engageCheckGizmosRayDistance = hit.distance;
            if (hit.collider.GetComponent<Player>() != null)
            {
                engageCheckGizmosRayDistance = hit.distance;
                shouldAttack = true;

            }
            else
            {

                shouldAttack = false;
            }
        }
        else
        {
            engageCheckGizmosRayDistance = engageCheckRayDistance;
            shouldAttack = false;
        }
       
    }

    public void CastPatrolCheck()
    {
        RaycastHit2D forwardHit = Physics2D.Raycast(boxCollider2D.bounds.center, (isFacingLeft) ? Vector2.left : Vector2.right, patrolCheckRayDistance, patrolCheckMask);
        if (forwardHit.collider != null)
        {
            shouldTurnBack = true;
        }
        else
        {
            RaycastHit2D downHit = Physics2D.Raycast(boxCollider2D.bounds.center + (Vector3)((isFacingLeft) ? Vector2.left * patrolCheckRayDistance : Vector2.right * patrolCheckRayDistance), Vector2.down, patrolCheckRayDistance, patrolCheckMask);
            if (downHit.collider != null)
            {
                shouldTurnBack = false;
            }
            else
            {
                shouldTurnBack = true;
            }
        }

    }
    public void TurnBack()
    {
        shouldTurnBack = false;
        if (isFacingLeft)
        {
            isFacingLeft = false;
            spriteRenderer.flipX = true;
        }
        else
        {
            isFacingLeft = true;
            spriteRenderer.flipX = false;
        }
    }
    public void FollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 playerPos = player.transform.position;
        Vector3 enemyPos = transform.position;

        // Kiểm tra xem player nằm bên trái hay phải
        if (playerPos.x < transform.position.x && !isFacingLeft)
        {
            isFacingLeft = true;
            spriteRenderer.flipX = false;
        }
        else if (playerPos.x > transform.position.x && isFacingLeft)
        {
            isFacingLeft = false;
            spriteRenderer.flipX = true;
        }
    }


    private void OnDrawGizmos()
    {
        if (shouldAttack)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        
        Vector3 start = boxCollider2D.bounds.center;
        Vector3 end = start + ((isFacingLeft) ? Vector3.left * engageCheckGizmosRayDistance : Vector3.right * engageCheckGizmosRayDistance);
        Gizmos.DrawLine(start, end);

        Gizmos.color = Color.black;
        Gizmos.DrawLine(start, start + ((isFacingLeft) ? Vector3.left * patrolCheckGizmosRayDistance : Vector3.right * patrolCheckGizmosRayDistance));
    }

    public IEnumerator RandomPatrolRun()
    {
        while (true)
        {
            shouldRun = true;
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            shouldRun = false;
            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
        }
    }

    public void Damage(float dmg)
    {
        stats.health -= dmg;
        animator.Play("Hit");
        isDamaged = true;
        if (stats.health <= 0)
        {
            isDie = true;
            boxCollider2D.isTrigger = true;
        }
    }
}
