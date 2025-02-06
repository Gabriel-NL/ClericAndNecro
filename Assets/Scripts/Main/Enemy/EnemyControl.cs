using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject tombstone_prefab; // The tombstone prefab to spawn
    public Transform spawnParent; // Parent (SpawnPosition)
    public Collider groundCollider; // The collider defining the spawn area
    public LayerMask obstacleLayer; // Layer for collision checking
    public float margin; // Margin from collider edges

    private float checkRadius = 10f; // Collision check radius
    private int maxAttempts = 20; // Max spawn attempts


    private List<(int, GameObject)> created_tombstones;
    public int tombstone_limit = 10;
    public float internal_clock = 1.0f;
    public int tombstone_lifetime = 1;

    public GameObject enemy_prefab; // The tombstone prefab to spawn

    private List<GameObject> enemies_generated = new List<GameObject>();
    public int max_number_enemies = 10;
    public float enemy_spawn_radius = 4f;
    public GameObject enemy_list_parent;

    void Start()
    {

        Vector3 minBounds, maxBounds;
        (minBounds, maxBounds) = CalculateSpawnBounds();

        created_tombstones = new List<(int, GameObject)>();
        StartCoroutine(Spawner(minBounds, maxBounds));

    }

    private (Vector3, Vector3) CalculateSpawnBounds()
    {
        // Get the collider bounds
        Bounds bounds = groundCollider.bounds;

        // Apply margin
        Vector3 minBounds = new Vector3(bounds.min.x + margin, bounds.min.y, bounds.min.z + margin);
        Vector3 maxBounds = new Vector3(bounds.max.x - margin, bounds.max.y, bounds.max.z - margin);
        return (minBounds, maxBounds);
    }

    private IEnumerator Spawner(Vector3 min_bounds, Vector3 max_bounds)
    {

        while (true)
        {
            UpdateTombstoneLifetime();

            // Check if we can spawn a new object
            if (created_tombstones.Count < tombstone_limit)
            {
                GameObject tombstone = SpawnTombstone(min_bounds, max_bounds);
                if (tombstone != null)
                {
                    created_tombstones.Add((5, tombstone));
                }
            }
            else
            {
                //Debug.Log("Limit reached");
            }
            SpawnEnemiesAroundTombstones();



            yield return new WaitForSeconds(internal_clock);
        }
    }




    private GameObject SpawnTombstone(Vector3 min_bounds, Vector3 max_bounds)
    {
        List<Vector3> forbidden_coordinates = new List<Vector3>();
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random position within bounds
            float randomX = Random.Range(min_bounds.x, max_bounds.x);
            float randomZ = Random.Range(min_bounds.z, max_bounds.z);
            Vector3 spawnPos = new Vector3(randomX, 2, randomZ);

            if (forbidden_coordinates.Contains(spawnPos))
            {
                continue;
            }
            else
            {
                forbidden_coordinates.Add(spawnPos);
            }
            // Check for obstacles
            bool isClear = Physics.OverlapSphere(spawnPos, checkRadius, obstacleLayer).Length == 0;

            if (isClear)
            {
                // Instantiate and set as child of spawnParent
                GameObject tombstone = Instantiate(tombstone_prefab, spawnPos, Quaternion.identity);
                tombstone.transform.SetParent(spawnParent, worldPositionStays: true);
                tombstone.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

                return tombstone;
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position.");
        return null;
    }

    private void UpdateTombstoneLifetime()
    {
        int index = 0;
        int size = created_tombstones.Count;
        while (index < size)
        {
            size = created_tombstones.Count;
            if (created_tombstones[index].Item1 <= 0)
            {
                Destroy(created_tombstones[index].Item2);
                created_tombstones.RemoveAt(index);
                continue;
            }
            else
            {
                created_tombstones[index] = (created_tombstones[index].Item1 - 1, created_tombstones[index].Item2);
            }

            index += 1;
        }
    }

    private void SpawnEnemiesAroundTombstones()
    {
        List<Vector3> forbiddenCoordinates = new List<Vector3>();
        foreach (var (lifetime, tombstoneObj) in created_tombstones)
        {
            if (enemies_generated.Count >= max_number_enemies)
            {
                break;

            }

            Vector3 tombstonePosition = tombstoneObj.transform.position;
            for (int j = 0; j < maxAttempts; j++)
            {
                // Random position within the square radius around the tombstone
                float randomX = Random.Range(tombstonePosition.x - enemy_spawn_radius, tombstonePosition.x + enemy_spawn_radius);
                float randomZ = Random.Range(tombstonePosition.z - enemy_spawn_radius, tombstonePosition.z + enemy_spawn_radius);
                Vector3 spawnPos = new Vector3(randomX, tombstonePosition.y, randomZ); // Same Y level as tombstone
                                                                                       // Check if this position is forbidden (already spawned something here)
                if (forbiddenCoordinates.Contains(spawnPos))
                {
                    continue;
                }
                else
                {
                    forbiddenCoordinates.Add(spawnPos);
                }
                // Check for obstacles
                bool isClear = Physics.OverlapSphere(spawnPos, enemy_spawn_radius, obstacleLayer).Length == 0;

                if (isClear)
                {
                    // Instantiate the enemy and set as child of the tombstone (or other parent)
                    GameObject enemy = Instantiate(enemy_prefab, spawnPos, Quaternion.identity);
                    //enemy.transform.SetParent(tombstoneObj.transform, worldPositionStays: true);

                    enemies_generated.Add(enemy);
                    enemy.transform.SetParent(enemy_list_parent.transform, true);
                    break; // Exit the inner loop once an enemy has been successfully spawned
                }
            }
            //Debug.Log("No valid position to spawn enemies for this tombstone");

        }

    }

}
