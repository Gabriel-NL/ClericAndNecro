using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    [Header("MovementAttributes")]
    public float moveSpeed = 5f; // Speed of the player's movement
    public float dashForce = 50f; // Force applied during dash
    private float dashCooldown = 3f; // Cooldown between dashes
    private Vector3 movement; // Store the player's movement input
    [SerializeField]private float lastDashTime=0f;
    public Rigidbody rb; // Reference to the Rigidbody2D component
    public Animator anim;

    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the player object!");
        }
    }

    public void HandleMovement()
    {
        // Get input from the player (WASD or arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // Left/right input
        movement.z = Input.GetAxisRaw("Vertical");   // Up/down input

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        if (movement!= Vector3.zero)
        {
             anim.SetBool("walk", true);
        }else
        {
            anim.SetBool("walk",false);
        }
        
        
    }

    void HandleDash()
    {
        if (lastDashTime>0)
        {
            lastDashTime-=Time.deltaTime;
        }
        // Dash input (e.g., pressing 'Shift')
        if (Input.GetKeyDown(KeyCode.LeftShift) && lastDashTime<=0)
        {
            lastDashTime = dashCooldown; // Update the last dash time
            Debug.Log("Dashing");

            // Apply force for the dash
            rb.AddForce(movement * dashForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        HandleMovement();
        HandleDash();
        rb.velocity = movement * moveSpeed;
    }
}
