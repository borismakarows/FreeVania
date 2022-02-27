using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    PlayerMovement playerMovement;
    EnemyMovement enemyMovement;
    Rigidbody2D axeRb2D;
    [SerializeField] Vector2 axeVel = new Vector2(0f,0f);
    [SerializeField] GameObject erza;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float gravityMultiply = 1.1f;
    [SerializeField] float destroyDelay = 3f;
    [SerializeField] float axeDestroyDelay = 1f;
    Transform startPosition;
    PlayerMovement player;
    bool canThrow = true;
    Transform playerTransform;
    float xSpeed;
    SpriteRenderer axeSprite;
    Animator enemyAnimator;

    void Start()
    {
        axeRb2D = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        playerTransform = player.GetComponent<Transform>();
        xSpeed = playerTransform.localScale.x * axeVel.x;
        axeSprite = GetComponent<SpriteRenderer>();
        startPosition = transform;
        enemyMovement = FindObjectOfType<EnemyMovement>();
        playerMovement = FindObjectOfType<PlayerMovement>();       
    }
    void Update()
    {
        throwAxe();
    }

    void throwAxe()
    {
        if (Mathf.Sign(xSpeed) < 0)
        {
            axeSprite.flipX = true;
            rotationSpeed += 0.45f;
        }
        if (canThrow)
        {
            transform.eulerAngles += Vector3.forward * rotationSpeed;
            axeRb2D.gravityScale += gravityMultiply * Time.deltaTime;
            axeRb2D.velocity = new Vector2(xSpeed,axeVel.y);
        }
        else {return;} 
    }

    void OnCollisionEnter2D(Collision2D other) {
        
        if (other.gameObject.tag == "Ground")
        {
            canThrow = false;
            axeRb2D.velocity = new Vector2(0f,0f);
        }
        Destroy(gameObject,axeDestroyDelay);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Enemy" && canThrow == true)
        {
            Destroy(other.gameObject,destroyDelay);
            Destroy(gameObject,destroyDelay);
            canThrow = false;
            SpriteRenderer enemySprite = other.GetComponent<SpriteRenderer>();
            Rigidbody2D enemyRb2D = other.GetComponent<Rigidbody2D>();
            enemyAnimator = other.GetComponent<Animator>();
            enemySprite.color = enemyMovement.deathColor;
            enemyAnimator.SetBool("Dead", true);
            enemyRb2D.velocity = new Vector2(0f,0f);
            enemyRb2D.constraints = RigidbodyConstraints2D.FreezeAll;
            axeRb2D.velocity = new Vector2(0f,0f);
            axeRb2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}
