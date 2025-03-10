
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [Header("Initial declaration")]
    public GameObject tombstone_prefab; // The tombstone prefab to spawn
    public Transform spawnParent; // Parent (SpawnPosition)
    public Collider groundCollider; // The collider defining the spawn area
    public LayerMask obstacleLayer; // Layer for collision checking

    [Header("MapConfigs")]
    public float margin; // Margin from collider edges
    public float checkRadius = 10f; // Collision check radius
    public int maxAttempts = 20; // Max spawn attempts
    private Vector3 min_bounds, max_bounds;
    public int wave = 0;

    [Header("Tombstone Configs")]
    private List<GameObject> created_tombstones;
    private int tombstone_limit = 1;
    public float tombstone_internal_clock = 1.5f;

    [Header("Enemy Configs")]
    public GameObject enemy_prefab; // The tombstone prefab to spawn
    private List<GameObject> enemies_generated = new List<GameObject>();
    public float enemy_internal_clock = 1.0f;
    private int max_number_enemies = 20;
    public float enemy_spawn_radius = 4f;
    public GameObject enemy_list_parent;
    public bool canSpawn;

    void Start()
    {
        if (tombstone_prefab == null)
        {
            throw new System.ArgumentNullException(nameof(tombstone_prefab), "Tombstone prefab is null!");
        }

        (min_bounds, max_bounds) = CalculateSpawnBounds();

        created_tombstones = new List<GameObject>();
        StartCoroutine(TombstoneSpawner());
        StartCoroutine(EnemySpawner());
        wave = 1;
    }

    private (Vector3, Vector3) CalculateSpawnBounds()
    {
        // Get the collider bounds
        Bounds bounds = groundCollider.bounds;

        // Apply margin
        Vector3 min_bounds = new Vector3(bounds.min.x + margin, bounds.min.y, bounds.min.z + margin);
        Vector3 max_bounds = new Vector3(bounds.max.x - margin, bounds.max.y, bounds.max.z - margin);
        return (min_bounds, max_bounds);
    }

    private IEnumerator TombstoneSpawner()
    {

        while (created_tombstones.Count < tombstone_limit)
        {
            created_tombstones.Add(SpawnTombstone(min_bounds, max_bounds));

            yield return new WaitForSeconds(tombstone_internal_clock);
        }
    }

    private GameObject SpawnTombstone(Vector3 min_bounds, Vector3 max_bounds)
    {
        float randomX;
        float randomZ;
        Vector3 spawnPos;
        List<Vector3> forbidden_coordinates = new List<Vector3>();
        bool isClear;
        GameObject tombstone;
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random position within bounds
            randomX = Random.Range(min_bounds.x, max_bounds.x);
            randomZ = Random.Range(min_bounds.z, max_bounds.z);
            spawnPos = new Vector3(randomX, tombstone_prefab.transform.position.y, randomZ);

            if (forbidden_coordinates.Contains(spawnPos))
            {
                continue;
            }
            else
            {
                forbidden_coordinates.Add(spawnPos);
            }
            // Check for obstacles
            isClear = Physics.OverlapSphere(spawnPos, checkRadius, obstacleLayer).Length == 0;

            if (isClear)
            {
                // Instantiate and set as child of spawnParent
                tombstone = Instantiate(tombstone_prefab, spawnPos, tombstone_prefab.transform.rotation);
                tombstone.transform.SetParent(spawnParent, worldPositionStays: true);

                return tombstone;
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position.");
        return null;
    }

    public void OnTombstoneDestroy(GameObject destroyed_obj)
    {

        created_tombstones.Remove(destroyed_obj);
        if (created_tombstones.Count <= 0)
        {
            wave += 1;
            tombstone_limit = wave;
            StartCoroutine(TombstoneSpawner());
        }
    }

        private IEnumerator EnemySpawner()
    {

        while (canSpawn)
        {
            SpawnEnemiesAroundTombstones();

            yield return new WaitForSeconds(enemy_internal_clock);
        }
    }

    private void SpawnEnemiesAroundTombstones()
    {

        Vector3 tombstonePosition, spawnPos;
        float randomX, randomZ;
        bool isClear;
        GameObject enemy;
        double extra_hp = 0;
        Bounds bounds = tombstone_prefab.GetComponent<Renderer>().bounds;
        float checkRadius = Mathf.Max(bounds.extents.x, bounds.extents.z);

        foreach (GameObject tombstoneObj in created_tombstones)
        {
            if (enemies_generated.Count >= max_number_enemies)
            {
                break;
            }
            for (int i = 0; i < 20; i++)
            {

                tombstonePosition = tombstoneObj.transform.position;
                // Random position within the square radius around the tombstone
                randomX = Random.Range(
    tombstonePosition.x - enemy_spawn_radius - bounds.extents.x,
    tombstonePosition.x + enemy_spawn_radius + bounds.extents.x
                );
                randomZ = Random.Range(
                    tombstonePosition.z - enemy_spawn_radius - bounds.extents.z,
                    tombstonePosition.z + enemy_spawn_radius + bounds.extents.z
                );
                spawnPos = new Vector3(randomX, tombstonePosition.y, randomZ);

                // Check for obstacles
                isClear = Physics.OverlapSphere(spawnPos, enemy_spawn_radius, obstacleLayer).Length == 0;

                if (isClear)
                {
                    // Instantiate the enemy and set as child of the tombstone (or other parent)
                    enemy = Instantiate(enemy_prefab, spawnPos, Quaternion.identity);
                    //enemy.transform.SetParent(tombstoneObj.transform, worldPositionStays: true);
                    
                    enemies_generated.Add(enemy);
                    enemy.transform.SetParent(enemy_list_parent.transform, true);
                    extra_hp = tombstoneObj.GetComponent<TombData>().GetExtraHP();
                    enemy.GetComponent<EnemyData>().AddExtraHP(extra_hp);
                    // Exit the inner loop once an enemy has been successfully spawned
                    break;
                }
                else
                {
                    continue;
                }
            }


        }

    }

    public void OnEnemyDestroy(GameObject destroyed_obj)
    {
        created_tombstones.Remove(destroyed_obj);
    }

}
