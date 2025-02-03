using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    
    public GameObject enemy_prefab;  // enemy_Prefab to spawn
    public float radius = 2.0f;  // Radius for random spawn positions
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
        // Generate a random angle
        float angle = Random.Range(0f, 360f);
        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

        // Instantiate the enemy_prefab at the random position
        GameObject new_enemy = Instantiate(enemy_prefab, spawnPosition, Quaternion.identity);
        new_enemy.GetComponent<EnemyData>().SetTombstoneParent(this);

        // Add the spawned object to the list
        spawnedObjects.Add(new_enemy);

        // If we hit the max objects limit, stop spawning
        if (spawnedObjects.Count >= maxObjects)
        {
            isSpawning = false;
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
