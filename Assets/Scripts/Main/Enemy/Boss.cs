using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    public float retreatDistance = 5f; // Distance at which the enemy starts retreating
    public float retreatSpeed = 3.5f;
    private int boss_hp=20;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
         if (player == null)
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
        {
            player = foundPlayer.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the correct tag.");
        }
    }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        if (distanceToPlayer < retreatDistance)
        {
            RetreatFromPlayer();
        }
    }
    
    void RetreatFromPlayer()
    {
        Vector3 directionAway = (transform.position - player.position).normalized;
        Vector3 retreatPosition = transform.position + directionAway * retreatDistance;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(retreatPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            navMeshAgent.speed = retreatSpeed;
            navMeshAgent.SetDestination(hit.position);
        }
    }
}
