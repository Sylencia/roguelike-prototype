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

    private Keyboard keyboard;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        keyboard = Keyboard.current;
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        if (keyboard != null)
        {
            float moveHorizontal = 0.0f;
            if (keyboard.dKey.isPressed)
            {
                moveHorizontal += (1.0f * speed);
            }
            if (keyboard.aKey.isPressed)
            {
                moveHorizontal -= (1.0f * speed);
            }

            rigidBody.velocity = new Vector2(moveHorizontal, rigidBody.velocity.y);
        }
    }

    private void Jump()
    {
        if (keyboard != null && keyboard.spaceKey.isPressed)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }

        if (rigidBody.velocity.y < 0)
        {
            rigidBody.velocity += Vector2.up * (-5.0f) * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidBody.velocity.y >= 0 && !keyboard.spaceKey.isPressed)
        {
            rigidBody.velocity += Vector2.up * (-5.0f) * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
