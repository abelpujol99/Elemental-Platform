using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public static bool jumpUpgrade;
    public static bool doubleJumpUpgrade;
    
    private float moveSpeed = 2;
    private float jumpSpeed = 3;
    private float doubleJumpSpeed = 2.5f;
    private float fallMultiplier = 0.5f;
    private float lowJumpMultiplier = 1f;
    private bool run;
    private bool jump;
    private bool fall;
    private bool doubleJump;
    private bool canDoubleJump;

    Rigidbody2D rb2D;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //jumpUpgrade = true;
        //doubleJumpUpgrade = true;
    }

    void Update()
    {

        Jump();
        
    }

    void FixedUpdate()
    {
        
        LeftRightMove();
        
        LowHighJump();
        
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Platform"))
        {
            CheckGround.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.CompareTag("Platform"))
        {
            CheckGround.isGrounded = false;
        }
    }

    private void LeftRightMove()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            spriteRenderer.flipX = false;
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
            run = true;
            animator.SetBool("Run", run);
        } 
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            spriteRenderer.flipX = true;
            rb2D.velocity = new Vector2(-moveSpeed, rb2D.velocity.y);
            run = true;
            animator.SetBool("Run", run);
        }
        else
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            run = false;
            animator.SetBool("Run", run);
        }
    }

    private void Jump()
    {
        if (jumpUpgrade)
        {

            if (CheckGround.isGrounded && !canDoubleJump)
            {
                canDoubleJump = true;
            }
            
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (CheckGround.isGrounded)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
                }
                else if (doubleJumpUpgrade && canDoubleJump && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                {
                    if (rb2D.velocity.y < 0)
                    {
                        rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpSpeed);
                        animator.SetBool("DoubleJump", canDoubleJump);
                        canDoubleJump = false;
                    }
                    else
                    {
                        doubleJump = true;
                        animator.SetBool("DoubleJump", doubleJump);
                        rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpSpeed);
                        doubleJump = false;
                        canDoubleJump = false;

                    }
                }
            }
    
            if (rb2D.velocity.y > 0 && !CheckGround.isGrounded)
            {
                run = false;
                jump = true;
                animator.SetBool("Run", false);
                animator.SetBool("Jump", true);
            }
            else if (rb2D.velocity.y < 0 && !CheckGround.isGrounded)
            {
                jump = false;
                run = false;
                fall = true;
                animator.SetBool("Jump", jump);
                animator.SetBool("Run", run);
                animator.SetBool("Fall", fall);
                animator.SetBool("DoubleJump", doubleJump);
            }
            else if (jump && CheckGround.isGrounded)
            {
                jump = false;
                animator.SetBool("Jump", jump);
            }
            else if (fall && CheckGround.isGrounded)
            {
                fall = false;
                animator.SetBool("Fall", fall);
            }
            
        }
    }

    private void LowHighJump()
    {
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if (rb2D.velocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }
    }
}
