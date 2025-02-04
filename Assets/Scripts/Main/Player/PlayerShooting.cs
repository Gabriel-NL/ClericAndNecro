using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the projectile to shoot
    public Transform firePoint; // Point where the projectile will be spawned
    public float projectileSpeed = 10f; // Speed of the projectile

    void Update()
    {

        // Rotate the player to face the mouse cursor

        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;

        // Validate if mousePosition is inside the screen bounds (non-negative values)
        if (float.IsNaN(mousePosition.x) || float.IsNaN(mousePosition.y))
        {
            
            return; // Exit early if the position is invalid
        }

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Shoot a projectile when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(direction);
        }
        

    }

    void Shoot(Vector2 direction)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            //projectile.transform.SetParent(firePoint);

        }
        else
        {
            Debug.LogError("ProjectilePrefab or FirePoint is not assigned in the Inspector.");
        }
    }
}
