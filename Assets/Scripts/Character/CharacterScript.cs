using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public static bool jumpUpgrade;
    public static bool doubleJumpUpgrade = false;
    
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float jumpSpeed = 3;
    [SerializeField] private float doubleJumpSpeed = 2.5f;
    [SerializeField] private float fallMultiplier = 0.5f;
    [SerializeField] private float lowJumpMultiplier = 1f;
    [SerializeField] private bool run;
    [SerializeField] private bool jump;
    [SerializeField] private bool fall;
    [SerializeField] private bool doubleJump;

    Rigidbody2D rb2D;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (jumpUpgrade)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (CheckGround.isGrounded)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
                }
                else if (doubleJumpUpgrade && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                {
                    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    {
                        doubleJump = true;
                        animator.SetBool("DoubleJump", doubleJump);
                        rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpSpeed);
                        doubleJump = false;
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

    void FixedUpdate()
    {
        //Move Rifht and Left
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
        
        //Lower and Higher Jump
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
