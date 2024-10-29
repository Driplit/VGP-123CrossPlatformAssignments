using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(GroundCheck), typeof(Jump), typeof(Shoot))]
public class PlayerController : MonoBehaviour
{
    //Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    GroundCheck gc;

    //Movement variables
    [Range(3, 10)]
    public float speed = 5.5f;
    [Range(3, 10)]
    public float jumpForce = 3f;

    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gc = GetComponent<GroundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        CheckIsGrounded();
        float hInput = Input.GetAxis("Horizontal");

        if (curPlayingClips.Length > 0)
        {
            if (!(curPlayingClips[0].clip.name == "Fire"))
            {
                rb.velocity = new Vector2(hInput * speed, rb.velocity.y);
            }

        }

        //sprite flipping
        if (hInput != 0) sr.flipX = (hInput < 0);

        //inputs for firing and jump attack
        if (Input.GetButtonDown("Fire1") && isGrounded) anim.SetTrigger("fire");
        
        //Jump attack will need later
        //if (Input.GetButtonDown("Fire1") && !isGrounded) anim.SetTrigger("jumpAttack");

        //alternate way to sprite flip
        //if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX) sr.flipX = !sr.flipX;

        anim.SetBool("Run", hInput != 0);
        anim.SetBool("Grounded", isGrounded);
        anim.SetBool("Falling",IsFalling());
        anim.SetBool("Rising",IsRising());
        
    }

    void CheckIsGrounded()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y <= 0) isGrounded = gc.IsGrounded();
        }
        else isGrounded = gc.IsGrounded();
    }
    bool IsFalling()
    {
        return rb.velocity.y < 0;
    }
    bool IsRising()
    {
        return rb.velocity.y > 0;
    }
}