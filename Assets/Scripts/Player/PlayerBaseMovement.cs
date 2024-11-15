using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpPower = 16f;

    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move() 
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpPower);
        }
    }
}
