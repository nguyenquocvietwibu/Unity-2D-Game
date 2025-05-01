using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float moveSpeed;
    public GameObjectPool pool;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb2D;
    private void Start()
    {
        
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="isRightDirection"> trái là false, phải là true</param>
    /// <param name=""></param>

    public void Shoot(bool isRightDirection)
    {
        if (isRightDirection)
        {
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
        }
        else
        {
            rb2D.velocity = new Vector2(-moveSpeed, rb2D.velocity.y);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SolidPlatform"))
        {
            pool.GoInPool(gameObject);
        }
    }
}
