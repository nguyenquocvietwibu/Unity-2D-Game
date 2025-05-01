using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float moveSpeed;
    public GameObjectPool pool;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2D;

    private void Awake()
    {
        moveSpeed = 10f;
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = rb2D.GetComponent<SpriteRenderer>();
        pool = GetComponentInParent<GameObjectPool>();
    }
    private void Start()
    {
        
    }
    public void Shoot(bool isLeftDirection)
    {
        if (isLeftDirection)
        {
            
            spriteRenderer.flipX = false;
            rb2D.velocity = new Vector2(-moveSpeed, 0);
        }
        else
        {
            spriteRenderer.flipX = true; ;
            rb2D.velocity = new Vector2(moveSpeed, 0);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SolidPlatform"))
        {
            pool.GoInPool(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.Damage(1f);
            }
            pool.GoInPool(gameObject);
        }
    }
}
