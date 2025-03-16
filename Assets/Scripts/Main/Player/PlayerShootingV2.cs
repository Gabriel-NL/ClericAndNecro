using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingV2 : MonoBehaviour
{
    [Header("Game objects")]
    public GameObject projectile_prefab;
    public Transform cast_point, staff, player, eyes;

    [Header("Public vars")]
    public float projectileSpeed = 10f; // Speed of the projectile
    public float fireRate = 0.5f; // Time between shots (in seconds)

    [Header("Variables Changed Dynamically")]
    [SerializeField] private Vector3 direction, mousePosition;
    public float fireTimer = 0f; // Timer to track when the next shot is allowed
    public float angle,eyeX,eyeY;
    public Vector3 distanceVector;





    void Update()
    {
        HandleShooting();
    }
    void FixedUpdate()
    {
        HandleItemRotation();
        HandleHandSwapping();
        HandleEyeMovement();
    }

    private void HandleItemRotation()
    {

        // Get the mouse position in screen space
        mousePosition = Input.mousePosition;

        if (float.IsInfinity(mousePosition.x) || float.IsInfinity(mousePosition.y) || float.IsInfinity(mousePosition.z))
        {
            return;
        }
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.y = player.position.y;

        // Calculate direction from player to mouse
        direction = (mousePosition - staff.position).normalized;

        // Get the angle to rotate on the Y-axis
        angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        staff.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    private void HandleHandSwapping()
    {
        if (angle < 0)
        {
            staff.localPosition = new Vector3(0.7f, staff.localPosition.y, staff.localPosition.z);
        }
        else
        {
            staff.localPosition = new Vector3(-0.7f, staff.localPosition.y, staff.localPosition.z);
        }
    }

    private void HandleEyeMovement()
    {
        distanceVector = mousePosition - staff.position;
        distanceVector.x = Mathf.Clamp(distanceVector.x, -7f, 7f);
        distanceVector.y = Mathf.Clamp(distanceVector.y, -7f, 7f);
        distanceVector.z = Mathf.Clamp(distanceVector.z, -7f, 7f);



         eyeX = Mathf.Lerp(-0.1f, 0.1f, (distanceVector.x + 7f) / 14f);
         eyeY = Mathf.Lerp(0.15f, 0.25f, (distanceVector.z + 7f) / 14f);

        // Apply movement
        eyes.localPosition = new Vector3(eyeX, eyeY, eyes.localPosition.z);
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
        if (projectile_prefab != null && cast_point != null)
        {

            GameObject projectile = Instantiate(projectile_prefab, cast_point.position, cast_point.rotation);
        }
        else
        {
            Debug.LogError("ProjectilePrefab or FirePoint is not assigned in the Inspector.");
        }
    }
}
