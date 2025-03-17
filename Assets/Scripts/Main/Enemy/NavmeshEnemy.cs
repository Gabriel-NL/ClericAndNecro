using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshEnemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform player;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {

        // Get the NavMeshAgent component on this enemy
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
        
        originalScale = transform.localScale;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Set the destination for the NavMeshAgent
            navMeshAgent.SetDestination(player.position);

            // Flip the enemy based on player's position
            if (player.position.x < transform.position.x)
            {
                // Face right
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else
            {
                // Face left
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
        }

    }

}
