using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyData : MonoBehaviour
{
    //public vars
    [Range(0, 100)] public int probability = 5;
    public int pointsWorth = 10;
    public GameObject healingItemPrefab; // Assign a HealingItem prefab in the Inspector

    //private game Object declaration
    private Transform eyes;

    //Private components
    private NavMeshAgent navMeshAgent;

    //private variables
    private int prob_result;
    private int health_points = 3, max_hp = 3;
    private Transform player;
    private bool facingRight = true;
    private Vector3 distanceVector, mousePosition;
    private float eyeX, eyeY;
    private BreakStateController breakStateController;
    public AudioClip skeletonDamage;
    public AudioClip skeletonDeath;



    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        eyes = transform.Find("Head").Find("Skeleton_face");
        if (eyes == null)
        {
            Debug.LogError("Eyes not found");
        }
        player = GameObject.FindWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found");
        }
        breakStateController = GetComponent<BreakStateController>();
        if (breakStateController == null)
        {
            Debug.LogError("BreakStateController not found");
        }
        navMeshAgent.SetDestination(player.position);
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
            breakStateController.NextState();
            PlayDamage();
        }
        else
        {
            prob_result = Random.Range(1, 101);
            if (prob_result <= probability)
            {
                Instantiate(healingItemPrefab, transform.position, Quaternion.Euler(90, 0, 0));
            }
            TriggerDeath();

        }
    }
    public void TriggerDeath()
    {
        FindFirstObjectByType<EnemyControl>().OnEnemyDestroy(gameObject);
        // Add score when enemy is killed
        ScoreManager.Instance.AddScore(pointsWorth);
        Destroy(gameObject);
        PlayDeath();
    }
    public void Healed(int heal_value)
    {
        health_points += heal_value;

        if (health_points > max_hp)
        {
            health_points = max_hp;
        }
        breakStateController.RevertDamage(heal_value);
    }

    public void AddExtraSpeed(double extra_speed)
    {
        navMeshAgent.speed += (float)extra_speed;
    }

    void FixedUpdate()
    {
        if (!navMeshAgent.pathPending)
        {
            navMeshAgent.SetDestination(player.position);
        }
        HandleEyeMovement();
    }


    private void HandleEyeMovement()
    {
        distanceVector = player.transform.position - transform.position;
        distanceVector.x = Mathf.Clamp(distanceVector.x, -7f, 7f);
        distanceVector.y = Mathf.Clamp(distanceVector.y, -7f, 7f);
        distanceVector.z = Mathf.Clamp(distanceVector.z, -7f, 7f);

        float eye_range_y = 0.09f, eye_range_x = 0.1f;

        eyeX = Mathf.Lerp(-eye_range_x, eye_range_x, (distanceVector.x + 7f) / 14f);
        eyeY = Mathf.Lerp(-eye_range_y, eye_range_y, (distanceVector.z + 7f) / 14f);

        // Apply movement
        eyes.localPosition = new Vector3(eyeX, eyeY, eyes.localPosition.z);
    }

    void PlayDamage()
    {
        if (skeletonDamage != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject audioObject = new GameObject("SkeletonDamage");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = skeletonDamage;
            audioSource.Play();

            // Destroy the temporary GameObject after the clip finishes playing
            Destroy(audioObject, skeletonDamage.length);
        }
    }

    void PlayDeath()
    {
        if (skeletonDeath != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject audioObject = new GameObject("SkeletonDeath");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = skeletonDeath;
            audioSource.Play();

            // Destroy the temporary GameObject after the clip finishes playing
            Destroy(audioObject, skeletonDeath.length);
        }
    }
}

