using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        if (float.IsNaN(mousePosition.x) || float.IsNaN(mousePosition.y))
        {
            
            return;
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot(direction);
        }
        

    }
    private void HandlePlayerRotation()
    {

        // Get the mouse position in screen space
        mousePosition = Input.mousePosition;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = player.transform.position.y;  // Set Y to player's Y to ignore vertical difference

        // Calculate direction from player to mouse
        direction = (mousePosition - player.transform.position).normalized;

        // Get the angle to rotate on the Y-axis
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        player.transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    private void HandleShooting(){
        
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
