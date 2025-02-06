using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
  
    public float moveSpeed = 5f; // Speed of the player's movement
    private Vector3 movement; // Store the player's movement input

    public Rigidbody rb; // Reference to the Rigidbody2D component


    
    void Start()
    {

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the player object!");
        }
    }



    void FixedUpdate()
    {
        // Get input from the player (WASD or arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // Left/right input
        movement.z = Input.GetAxisRaw("Vertical");   // Up/down input

        // Normalize movement to prevent faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        // Move the player using Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
