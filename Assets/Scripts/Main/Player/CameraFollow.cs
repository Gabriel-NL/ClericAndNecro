using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);


    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
