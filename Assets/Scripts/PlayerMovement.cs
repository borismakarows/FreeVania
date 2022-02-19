using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Game Objects")]
    Rigidbody2D erzaRigidBody;
    Animator erzaAnimator;
    Vector2 moveInput;
    CapsuleCollider2D erzaCapsuleCollider2D; 
    BoxCollider2D erzaBoxCollider2D;
    Vector2 startMoveInput;
    bool onAir;  
    bool isAlive = true;
    PlayerInput erzaPlayerInput;
    [SerializeField] CinemachineVirtualCamera deathCam;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    SpriteRenderer erzaSpriteRenderer;
    [SerializeField] Transform axePosition;
    [SerializeField] GameObject axeThrow;

    [Header("Attributes")]
    [SerializeField] float runSpeed = 2f;
    [SerializeField] float jumpSpeed = 4f;
    [SerializeField] float climbSpeed = 1f;

    [SerializeField] float maxZoom = 1.4f;
    [SerializeField] float max_intensity = 5f;
    [SerializeField] float shakeTimer = 2f;
    [SerializeField] Color32 deathColor;
    float gravityScaleatStart;
    [SerializeField] Vector3 throwSpeed = new Vector3(1f,0f,0f);
    
    
    void Start()
    {
        erzaSpriteRenderer = GetComponent<SpriteRenderer>();
        erzaRigidBody = GetComponent<Rigidbody2D>();
        erzaAnimator = GetComponent<Animator>();
        erzaCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        erzaBoxCollider2D = GetComponent<BoxCollider2D>();
        gravityScaleatStart = erzaRigidBody.gravityScale;
        Vector2 startMoveInput = moveInput;
        erzaPlayerInput = GetComponent<PlayerInput>();
        cinemachineBasicMultiChannelPerlin = deathCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        Run();
        Die();
        FlipSprite();
        ClimbLadder();    
        dontClimbWall();
        erzaAnimator.SetBool("IsJumping", onAir);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Ground")
        {
            onAir = false;
        }    
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "Ground")
        {
            onAir = true;
        }    
    }
    
    void FlipSprite()
    {
        
        bool hasPlayerMovesHorizontal = Mathf.Abs(erzaRigidBody.velocity.x) > Mathf.Epsilon;
        if (hasPlayerMovesHorizontal)
        {
        transform.localScale = new Vector2(Mathf.Sign(erzaRigidBody.velocity.x), 1f);
        }
        if (onAir == false)
        {
            erzaAnimator.SetBool("IsRunning", hasPlayerMovesHorizontal);
        }
        if (onAir == true)
        {
            erzaAnimator.SetBool("IsRunning", !onAir);
            erzaAnimator.SetBool("IsJumping", onAir);
        }
        
    }

    void OnMove(InputValue movevalue)
    {
        moveInput = movevalue.Get<Vector2>();
    }

    void Run()
    {        
        Vector2 startMoveInput = moveInput;
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, erzaRigidBody.velocity.y);
        erzaRigidBody.velocity = playerVelocity;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && onAir == false)
        {
            erzaRigidBody.velocity += new Vector2(0f , jumpSpeed);
        }
        
        
    }
    
    void OnAttack(InputValue value)
    {
        
        if (value.isPressed && onAir == false)
        {   
            erzaAnimator.SetBool("IsRunning", false);
            erzaAnimator.SetTrigger("IsAttacking");
            moveInput = new Vector2(0,0);
        }
        moveInput = startMoveInput;
    }

    void ClimbLadder()
    {
        
        if(Mathf.Abs(moveInput.y) < Mathf.Epsilon)
        {return;}
        else
        {   
            
            if (!erzaCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("ClimbingLayer"))) 
            {   
            erzaAnimator.SetBool("IsClimbing", false);
            erzaRigidBody.gravityScale = gravityScaleatStart;
            }
            
            else if(erzaCapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("ClimbingLayer")))
            {
            onAir = false;
            Vector2 climbVelocity = new Vector2(erzaRigidBody.velocity.x,moveInput.y * climbSpeed);
            erzaRigidBody.velocity = climbVelocity;
            erzaAnimator.SetBool("IsClimbing",true);
            erzaRigidBody.gravityScale = 0f;
            }
            
        }              
    }
    
    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Climbing")
        {
            erzaAnimator.SetBool("IsClimbing", false);
            erzaRigidBody.gravityScale = gravityScaleatStart;
            erzaAnimator.SetBool("IsJumping", false);
        }
    }

    void dontClimbWall()
    {

        if (erzaBoxCollider2D.IsTouchingLayers(LayerMask.GetMask("PlatformLayer")))
        {
            return;
        }
        
    }

    public Vector2 getMoveInput()
    {
        return moveInput;
    }


    void Die()
    {
        if (erzaRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazard")))
        {
            isAlive = false;
        }
         if (isAlive == false )
        {
            erzaPlayerInput.actions.Disable();
            erzaAnimator.SetBool("IsDead", !isAlive);
            shakeCameraAndZoom();
        }
        
    }


    void shakeCameraAndZoom()
    {
        erzaSpriteRenderer.color = deathColor;
        
        if (shakeTimer >= 0)
        {
            shakeTimer -= Time.deltaTime;
        }

        if(shakeTimer>0f)
        {   
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =  max_intensity;
            if (deathCam.m_Lens.OrthographicSize > maxZoom)
            {
                deathCam.m_Lens.OrthographicSize -= 1f;
            }

        }
        else
        {                
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        }
        
    }

    public bool getIsAlive()
    {
        return isAlive;
    }
    
    void OnThrow()
    {
        if(!isAlive) {return;}
        else
        {
            Instantiate(axeThrow,axePosition.position,transform.rotation);
        }
        
    
    
    }
}
