using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public GameObject necro_magic_prefab, obstacle_prefab;
    // Start is called before the first frame update
    private Transform player;
    private EnemyControl enemy_control_script;
    private Transform current_tomb;

    private int boss_hp = 20;
    private NavMeshAgent navMeshAgent;

    private float magic_fire_rate = 2.5f;
    private float magic_speed = 9f;
    private int obstacle_layer=7;


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
        StartCoroutine(StartShootingMagic());


    }

    public IEnumerator StartShootingMagic()
    {
        // Wait for the fire rate time before firing the next projectile
        yield return new WaitForSeconds(magic_fire_rate);
        while (true)
        {
            // Get the player's current position at the time of firing
            Vector3 playerPosition = player.transform.position;

            // Instantiate the magic projectile (make sure you have a reference to the magic prefab)
            GameObject magicProjectile = Instantiate(necro_magic_prefab, transform.position, Quaternion.identity);

            // Calculate the direction to the player's position
            Vector3 direction = (playerPosition - transform.position).normalized;

            // Assuming the magic projectile has a Rigidbody2D and a speed variable
            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * magic_speed;
            }

            // Destroy the projectile after it reaches its destination (e.g., a timer or distance check)
            float distance = Vector3.Distance(transform.position, playerPosition);
            float timeToReach = distance / magic_speed;  // Time = Distance / Speed


            // Wait until the projectile reaches its target position or time runs out
            yield return new WaitForSeconds(timeToReach);

            Vector3 explosion_center = magicProjectile.transform.position;
            float explosion_radius = 4.0f;
            int skeleton_layer = 8;
            Collider[] hit_skeletons = Physics.OverlapSphere(explosion_center, explosion_radius, skeleton_layer);

            if (hit_skeletons.Length > 0)
            {
                foreach (Collider hit in hit_skeletons)
                {
                    Instantiate(obstacle_prefab, hit.transform.position, Quaternion.identity); // Place obstacle at explosion center
                    Destroy(hit.gameObject); // Destroy skeletons

                }
                Destroy(magicProjectile);
                continue;
            }else
            {
                 Vector3 spawnPos = FindNearestAvailablePosition(explosion_center, explosion_radius/2);
            Instantiate(obstacle_prefab, spawnPos, Quaternion.identity);
            }

   

            // Destroy the projectile once it reaches the target position

        }

    }
    private Vector3 FindNearestAvailablePosition(Vector3 center, float radius)
    {
        Vector3 bestPos = center;
        float bestDistance = float.MaxValue;

        for (int i = 0; i < 36; i++) // Check around in 10-degree increments
        {
            float angle = i * 10 * Mathf.Deg2Rad;
            Vector3 checkPos = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            if (Physics.OverlapSphere(checkPos, 1f, obstacle_layer).Length == 0)
            {
                float distance = Vector3.Distance(center, checkPos);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestPos = checkPos;
                }
            }
        }

        return bestPos;
    }

    public void RandomNewTombstone()
    {
        enemy_control_script = FindFirstObjectByType<EnemyControl>();
        List<Transform> all_tombstones = enemy_control_script.GetTombstoneTransforms().ToList();

        if (all_tombstones.Count > 0)
        {
            int random_index = Random.Range(0, all_tombstones.Count);
            current_tomb = all_tombstones[random_index];
            transform.SetParent(current_tomb);
            gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            //Remove parent
            transform.SetParent(null, true);
            Debug.Log("Victory");
            enemy_control_script.Victory();

        }
    }

    public void SetPosition(Vector3 new_pos)
    {
        gameObject.transform.localPosition = new_pos;
    }
    public Transform GetCurrentTomb()
    {
        return current_tomb;
    }

}
