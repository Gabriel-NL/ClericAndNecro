using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Attributes")]
    public float moveSpeed = 5f;

    public Vector3 movement;

    public Rigidbody rb;
    public Animator anim;

    void Start()
    {
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on the player object!");
        }
    }

    public void HandleMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        anim.SetBool("walk", movement != Vector3.zero);
    }

    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
}
