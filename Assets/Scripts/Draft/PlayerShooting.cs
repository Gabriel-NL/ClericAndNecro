using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile to shoot
    public Transform firePoint; // Point where the projectile will be spawned
    public float projectileSpeed = 10f; // Speed of the projectile
    public GameObject player;
    private Vector3 direction, mousePosition;
    private float angle;

    public float fireRate = 0.5f; // Time between shots (in seconds)
    private float fireTimer = 0f; // Timer to track when the next shot is allowed


    void Update()
    {
        HandlePlayerRotation();
        HandleShooting();

    }
    private void HandlePlayerRotation()
    {

        // Get the mouse position in screen space
        mousePosition = Input.mousePosition;

        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y) || float.IsInfinity(mousePosition.z))
        {
            return;
        }
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = player.transform.position.y;  // Set Y to player's Y to ignore vertical difference

        // Calculate direction from player to mouse
        direction = (mousePosition - player.transform.position).normalized;

        // Get the angle to rotate on the Y-axis
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        player.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    private void HandleShooting()
    {

        // Decrease the fireTimer based on time passed
        fireTimer -= Time.deltaTime;

        // Shoot a projectile when the left mouse button is clicked or held down, respecting the fire rate
        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            // Shoot projectile
            Shoot(direction);

            // Reset the fireTimer to the fireRate
            fireTimer = fireRate;
        }
    }
    void Shoot(Vector2 direction)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogError("ProjectilePrefab or FirePoint is not assigned in the Inspector.");
        }
    }
}
