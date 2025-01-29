using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player's movement
    private Vector2 movement; // Store the player's movement input

    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the player object!");
        }
    }

    void Update()
    {
        // Get input from the player (WASD or arrow keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // Left/right input
        movement.y = Input.GetAxisRaw("Vertical");   // Up/down input

        // Normalize movement to prevent faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}