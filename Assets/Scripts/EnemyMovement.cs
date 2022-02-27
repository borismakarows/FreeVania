using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] PlayerMovement playerMovement;
    Rigidbody2D enemyRigidBody;
    [SerializeField] float enemySpeed = 0.5f;
    [SerializeField] Rigidbody2D childsRigidBody;   
    public Color32 deathColor;
    SpriteRenderer enemySprite;
    bool turn = true; 
    float delaytoDontFall = 2f;
    bool startTimer;
    float startTime;
    float walkaWhileTimer = 2f;
    public Animator enemyAnimator;
    

    void Start() 
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        startTime = delaytoDontFall;
        enemyAnimator = GetComponent<Animator>();
        enemySprite = GetComponent<SpriteRenderer>();
        if (transform.localScale.x == -1)
        {
            enemySpeed = -enemySpeed;
        }
    }

    void Update()
    {      
        if (playerMovement.getIsAlive() == true)
        {   
            enemyRigidBody.velocity = new Vector2(enemySpeed, enemyRigidBody.velocity.y);
            dontFall();
            startCounter();
        }
        else
        {
            if (walkaWhileTimer > 0) 
            {
                walkaWhileTimer -= Time.deltaTime;
                enemyRigidBody.velocity = new Vector2(enemySpeed,enemyRigidBody.velocity.y);
            }
            else
            {
                enemyRigidBody.velocity = new Vector2(0f,0f);
            }
            
        }
    }


    void flipFace()
    {
        transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x),1f,1f);
    }

    void dontFall()
    {
       if (!childsRigidBody.IsTouchingLayers(LayerMask.GetMask("PlatformLayer")) && turn == true)
       {
           enemySpeed = -enemySpeed;
           flipFace(); 
           startTimer = true;
       }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Attack")
        {
            enemySprite.color = deathColor;
            enemyAnimator.SetBool("Dead", true);
            enemyRigidBody.velocity = new Vector2(0f,0f);
            enemyRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 0.5f);
            Destroy(other.gameObject, playerMovement.attackZoneDuration);
        }
        
        if (enemyRigidBody.IsTouchingLayers(LayerMask.GetMask("PlatformLayer")))
        {
            enemySpeed = -enemySpeed;
            flipFace();    
        }
    }

    void startCounter()
    {
        
        if (startTimer == true)
        {
            delaytoDontFall -= Time.deltaTime;
            turn = false;
            if (delaytoDontFall <= 0)
            {
                delaytoDontFall = startTime;
                startTimer = false;
                turn = true;
            }
        }
    }


}
