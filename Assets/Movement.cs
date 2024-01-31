using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float aceleration = 5f;
    public float maxSpeed = 2f;
    public float jumpForce = 20f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;
    public float jumpvariableHeightMultiplier = 0.5f;
    public float gravity = 20f;
    public float fallMultiplier = 2.5f;
    public int jumpsAllowed = 2;
    public Rigidbody2D rb;
    public Collider2D groundCollider;

    private float inputVelocity = 0f;
    private float coyoteTimeCounter = 0f;
    private float jumpBufferTimeCounter = 0f;
    private int jumpsLeft = 0;

    private int jumpCooldown = 0;



    void FixedUpdate()
    {
        HandleBasicMovement();
        HandleJump();
    }

    private void HandleBasicMovement()
    {
        // Check input and adjusts inputVelocity
        if(Input.GetKey(KeyCode.D))
        {
            inputVelocity += aceleration;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            inputVelocity -= aceleration;
        }
        else
        {
            inputVelocity = 0f;
        }
        if(inputVelocity > maxSpeed)
        {
            inputVelocity = maxSpeed;
        }
        else if(inputVelocity < -maxSpeed)
        {
            inputVelocity = -maxSpeed;
        }

        //fast falling
        float vertricalVelocity = 0f;
        if(Input.GetKey(KeyCode.S)){
            vertricalVelocity = -fallMultiplier;
            jumpBufferTimeCounter = 0f;
        }

        rb.position += new Vector2(inputVelocity * Time.deltaTime, vertricalVelocity * Time.deltaTime);
        // Debug.Log("inputVelocity: " + inputVelocity);
    }

    private void HandleJump()
    {
        //TODO: Add jump buffer and implement Coyote Time and vaiable jump height
        if(Input.GetKey(KeyCode.Space))
        {
            if(IsGrounded() && (jumpCooldown <= 0)){
                jumpBufferTimeCounter = jumpBufferTime;
                jumpCooldown = 20;
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            else if(jumpBufferTimeCounter > 0f)
            {
                rb.AddForce(new Vector2(0f, jumpForce * jumpvariableHeightMultiplier), ForceMode2D.Impulse);
                jumpBufferTimeCounter -= Time.deltaTime;
            }
        }
        if(jumpCooldown != 0){
            jumpCooldown -= 1;
        }
    }
    

    

    private bool IsGrounded()
    {
        return groundCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
    }
}
