using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    private Rigidbody2D rigidBody;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private int extraJumps;
    public int extraJumpsValue;

    private Vector2 movementVector = Vector2.zero;

    void Start()
    {
        extraJumps = extraJumpsValue;
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CheckIfGrounded();
        Move();
        Fall();
    }

    public void MovementCallback(InputAction.CallbackContext context)
    {
        if(context.started || context.canceled)
        {
            movementVector = context.ReadValue<Vector2>();
        }
    }

    public void JumpCallback(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(extraJumps > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
                extraJumps--;
            }
            else if(extraJumps == 0 && isGrounded)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }
        }
    }

    private void CheckIfGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround) != null;

        if(isGrounded)
        {
            extraJumps = extraJumpsValue;
        }
    }

    private void Move()
    {
        rigidBody.velocity = new Vector2(movementVector.x * speed, rigidBody.velocity.y);

        if(!facingRight && movementVector.x > 0.0f)
        {
            Flip();
        }
        else if(facingRight && movementVector.x < 0.0f)
        {
            Flip();
        }
    }

    private void Fall()
    {
        if(rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.down * -Physics2D.gravity * fallMultiplier * Time.deltaTime;
        }
        else if(rigidBody.velocity.y > 0)
        {
            rigidBody.velocity += Vector2.down * -Physics2D.gravity * lowJumpMultiplier * Time.deltaTime;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
