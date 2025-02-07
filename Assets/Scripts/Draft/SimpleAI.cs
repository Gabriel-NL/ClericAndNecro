using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    private Transform player; 
    public float speed = 2.0f;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void FixedUpdate()

    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}
