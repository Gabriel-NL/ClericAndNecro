using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 3f;

    private float dashTimeRemaining;
    private float lastDashTime = 0f;
    private bool isDashing = false;
    public Image dashCooldownImage;  // Assign in Inspector

    private Movement movementScript;
    private Rigidbody rb;

    void Start()
    {
        movementScript = GetComponent<Movement>();
        rb = movementScript.rb;

        if (movementScript == null)
        {
            Debug.LogError("No Movement script found!");
        }
    }

    void Update()
    {
        HandleDash();
    }

    void HandleDash()
    {
        if (lastDashTime > 0)
    {
        lastDashTime -= Time.deltaTime;

        // Calculate transparency based on cooldown progress
        float alpha = Mathf.Clamp01(1 - (lastDashTime / dashCooldown));

        Color tempColor = dashCooldownImage.color;
        tempColor.a = alpha;
        dashCooldownImage.color = tempColor;
    }

    if (isDashing)
    {
        dashTimeRemaining -= Time.deltaTime;
        if (dashTimeRemaining <= 0)
        {
            isDashing = false;
        }
    }

    if (Input.GetKeyDown(KeyCode.LeftShift) && lastDashTime <= 0 && movementScript.movement != Vector3.zero)
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;
        lastDashTime = dashCooldown;

        // Make it fully transparent when dash starts
        Color tempColor = dashCooldownImage.color;
        tempColor.a = 0f;
        dashCooldownImage.color = tempColor;

        Debug.Log("Dashing!");
    }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = movementScript.movement * dashForce;
        }
    }
}
