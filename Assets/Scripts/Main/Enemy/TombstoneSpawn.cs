using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneSpawn : MonoBehaviour
{
    public GameObject tombstone_prefab;
    private float range = 7;
    private Vector2 tombstone_size;
    public float spawnTime = 2.0f;  // Time interval between spawns
    public int maxObjects = 4;  // Maximum number of objects allowed
    private List<GameObject> spawned_tombstones = new List<GameObject>();  // List to track spawned objects
    private bool is_spawning = true;  // Flag to control spawning



    void Start()
    {
        tombstone_size = tombstone_prefab.GetComponent<BoxCollider2D>().size;
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {

        while (true)
        {
            // Check if we can spawn a new object
            if (is_spawning && spawned_tombstones.Count < maxObjects)
            {
                SpawnTombstone();
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }




    void SpawnTombstone()
    {
        int maxAttempts = 10; // Avoid infinite loops
        int attempts = 0;
        Vector2 spawnPosition;

        while (attempts < maxAttempts)
        {
            // Pick a random position within the defined area
            spawnPosition = new Vector2(
                Random.Range(-range, range),
                Random.Range(-range, range)
            );

            // Check if the position is free
            if (!Physics2D.OverlapBox(spawnPosition, tombstone_size, 0))
            {
                // Spawn tombstone if no collisions
                Instantiate(tombstone_prefab, spawnPosition, Quaternion.identity);
                return;
            }

            attempts++; // Try again with a new position
        }

        Debug.LogWarning("Failed to find a valid spawn position for tombstone.");
    }
}
