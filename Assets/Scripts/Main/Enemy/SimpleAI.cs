using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    private Transform player;  // Assign the player GameObject in the Inspector
    public float speed = 2.0f; // Movement speed of AI
    void Start()
    {
        // Get the player object with the tag "Player" at the start
        player = GameObject.FindWithTag("Player").transform;
    }
    private void FixedUpdate()

    {
        if (player != null)
        {
            // Move AI towards the player position
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
