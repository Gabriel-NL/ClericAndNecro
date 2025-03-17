using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour
{
    [Range(0, 100)] public int probability = 5;
    private int prob_result;
    private double health_points = 3;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    public int pointsWorth = 10;
    public GameObject healingItemPrefab; // Assign a HealingItem prefab in the Inspector
    private bool facingRight = true;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (healingItemPrefab == null)
        {
            Debug.LogWarning("Healing prefab is missing!");
        }
        // Find the player by tag and get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
    }



    public void DealDamage(int damage)
    {
        if (health_points > damage)
        {
            health_points -= damage;
        }
        else
        {
            prob_result = Random.Range(1, 101);
            if (prob_result <= probability)
            {
                Instantiate(healingItemPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            }
            GameObject.FindFirstObjectByType<EnemyControl>().OnEnemyDestroy(gameObject);
            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(pointsWorth);
            Destroy(gameObject);
        }
    }

    public void AddExtraSpeed(float extra_speed)
    {
        navMeshAgent.speed += (float)extra_speed;
    }

    void FixedUpdate()
    {
        if (!navMeshAgent.pathPending && Vector3.Distance(navMeshAgent.destination, player.position) > 0.1f)
        {
            navMeshAgent.SetDestination(player.position);
        }
        
        FlipTowardsPlayer();
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        if ((player.position.x > transform.position.x && facingRight) ||
            (player.position.x < transform.position.x && !facingRight))
        {
            facingRight = !facingRight;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }
}

