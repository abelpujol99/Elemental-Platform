using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float moveSpeed = 2;
    public float jumpSpeed = 3;
    public float fallMultiplier = 0.5f;
    public float lowJumpMultiplier = 1f;
    public bool run;
    public bool jump;
    public bool fall;

    Rigidbody2D rb2D;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Move Rifht and Left
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            spriteRenderer.flipX = false;
            rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
            run = true;
            animator.SetBool("Run", run);
        } 
        else if (Input.GetKey("a") || Input.GetKey("left"))
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

        //Jump
        if (Input.GetKey("space") && CheckGround.isGrounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
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

        //Lower and Higher Jump
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if (rb2D.velocity.y > 0 && !Input.GetKey("space"))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }
    }
}
