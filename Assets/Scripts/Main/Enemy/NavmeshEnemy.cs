using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshEnemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        // Find the player by tag and get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the NavMeshAgent component on this enemy
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        
    }


    void FixedUpdate()
    {
        navMeshAgent.SetDestination(player.position);
    }

}
