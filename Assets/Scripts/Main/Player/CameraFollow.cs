using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform camera;
    public Transform player; // Assign the player's transform in the Inspector
    public float smoothSpeed = 5f; // Adjust for smoother movement
    public Vector3 offset = new Vector3(0f, 0f, -10f); // Keep a fixed Z value


    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
