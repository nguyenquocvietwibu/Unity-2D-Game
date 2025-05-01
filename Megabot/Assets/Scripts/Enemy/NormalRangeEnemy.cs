using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRangeEnemy : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Rigidbody2D rb2D;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public bool isFacingLeft;
    public GameObjectPool bulletPool;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (spriteRenderer.flipX)
        {
            isFacingLeft = false;
        }
        else
        {
            isFacingLeft = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
