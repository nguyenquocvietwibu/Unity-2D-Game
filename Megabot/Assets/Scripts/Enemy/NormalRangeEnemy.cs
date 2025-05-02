using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRangeEnemy : MonoBehaviour, IDamage
{
    public Rigidbody2D rb2D;
    public EnemyBullet bullet;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isFacingLeft;
    public GameObjectPool bulletPool;

    public Stats stats;

    public bool shouldShoot;

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
    public Vector3 leftPointShootVector3;
    public Vector3 rightPointShootVector3;

    public Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = false;

        stats = Instantiate(stats);
        bulletPool = GetComponentInChildren<GameObjectPool>();
        bulletPool.pooledGO = bullet.gameObject;
        bulletPool.PreWarm(10, bulletPool.transform);
        leftPointShootVector3 = bulletPool.transform.localPosition;

        leftPointShootVector3 = new Vector3(-Mathf.Abs(leftPointShootVector3.x), leftPointShootVector3.y);
        rightPointShootVector3 = new Vector3(Mathf.Abs(leftPointShootVector3.x), leftPointShootVector3.y);

        if (spriteRenderer.flipX)
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }
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
            if (shouldShoot)
            {
                animator.Play("Attack");
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

    public void ShootBullet()
    {

        bulletPool.transform.localPosition = (isFacingLeft) ?  leftPointShootVector3 : rightPointShootVector3;
        GameObject tempBulletGO = bulletPool.GoOutPool(bulletPool.transform);
        rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        EnemyBullet tempBullet = tempBulletGO.GetComponent<EnemyBullet>();
        if (tempBullet != null)
        {
            tempBullet.Shoot(isFacingLeft);
        }
    }

    public void CastEngagePlayerCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(boxCollider2D.bounds.center, (isFacingLeft) ? Vector2.left : Vector2.right, engageCheckRayDistance, engageCheckMask);
        if (hit.collider != null)
        {
            engageCheckGizmosRayDistance = hit.distance;
            if (hit.collider.GetComponent<Player>() != null)
            {
                engageCheckGizmosRayDistance = hit.distance;
                shouldShoot = true;
            }
            else
            {

                shouldShoot = false;
            }
        }
        else
        {
            engageCheckGizmosRayDistance = engageCheckRayDistance;
            shouldShoot = false;
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
    private void OnDrawGizmos()
    {
        if (shouldShoot)
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
        Gizmos.color = Color.black;
        Gizmos.DrawLine(boxCollider2D.bounds.center + (Vector3)((isFacingLeft) ? Vector2.left * patrolCheckRayDistance : Vector2.right * patrolCheckRayDistance), boxCollider2D.bounds.center + (Vector3)((isFacingLeft) ? Vector2.left * patrolCheckRayDistance : Vector2.right * patrolCheckRayDistance) - new Vector3(0, patrolCheckRayDistance, 0));
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
