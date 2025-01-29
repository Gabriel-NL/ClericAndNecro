using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;  // Force applied per frame
    public float maxSpeed = 5f;    // Maximum allowed velocity
    private Vector2 movementInput;
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found on the player object!");
        }
    }


      void MovementsHandler()
    {
        // Get input
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Only apply force when there is input
        if (movementInput != Vector2.zero)
        {
            movementInput.Normalize(); // Prevent diagonal speed boost

            // Apply force
            rb.AddForce(movementInput * moveSpeed, ForceMode2D.Force);
        }

        // Cap velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
       MovementsHandler();
    }
}