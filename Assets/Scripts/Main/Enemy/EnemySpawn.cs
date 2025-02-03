using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    
    public GameObject enemy_prefab;  // enemy_Prefab to spawn
    private float radius = 1.5f;  // Radius for random spawn positions
    public float spawnTime = 2.0f;  // Time interval between spawns
    public int maxObjects = 4;  // Maximum number of objects allowed
    private List<GameObject> spawnedObjects = new List<GameObject>();  // List to track spawned objects
    private bool isSpawning = true;  // Flag to control spawning
    void Start()
    {
        
        StartCoroutine(SpawnObjects()); 
    }

    // Coroutine to spawn objects at intervals
    private IEnumerator SpawnObjects()
    {

        while (true)
        {
            // Check if we can spawn a new object
            if (isSpawning && spawnedObjects.Count < maxObjects)
            {
                SpawnObject();
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
    private void SpawnObject()
    {
        int maxAttempts = 10; // Prevent infinite loops
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            // Generate a random angle in degrees
            float angle = Random.Range(0f, 360f);
            float angleRad = Mathf.Deg2Rad * angle;

            // Calculate the position (2D: X and Y)
            Vector3 angle_pos = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * radius;
            Vector3 spawnPosition = transform.position + angle_pos;

            // Check if the position is valid using a 2D raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, spawnPosition - transform.position, radius);

            if (!hit.collider) // If nothing is blocking the spawn position
            {
                GameObject new_enemy = Instantiate(enemy_prefab, spawnPosition, Quaternion.identity);
                new_enemy.GetComponent<EnemyData>().SetTombstoneParent(this);
                spawnedObjects.Add(new_enemy);

                // Stop spawning if we reach the max limit
                if (spawnedObjects.Count >= maxObjects)
                {
                    isSpawning = false;
                }

                break; // Exit the loop since we successfully spawned
            }

            attempts++; // Increase attempts to avoid infinite loops
        }
    }


    // Call this function when an object is killed, passing the object as a parameter
    public void ObjectKilled(GameObject killedObject)
    {
        // Remove the object from the list
        if (spawnedObjects.Contains(killedObject))
        {
            spawnedObjects.Remove(killedObject);
        }

        // Destroy the object
        Destroy(killedObject);

        // Resume spawning if we have fewer than the max allowed objects
        if (spawnedObjects.Count < maxObjects)
        {
            isSpawning = true;
        }
    }

}
