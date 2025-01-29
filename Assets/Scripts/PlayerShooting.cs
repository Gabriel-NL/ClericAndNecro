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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                projectileRb.velocity = direction * projectileSpeed;
            }
        }
        else
        {
            Debug.LogError("ProjectilePrefab or FirePoint is not assigned in the Inspector.");
        }
    }
}
