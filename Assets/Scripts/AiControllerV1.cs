using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiControllerV1 : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float speed = 3.0f; 
    public NavMeshAgent agent; 

    void Start()
    {
        agent.destination = player.position; 
        
    }

    void Update()
    {
        // Set the destination for the NavMeshAgent
    }
}
