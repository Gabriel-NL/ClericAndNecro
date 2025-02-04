using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject enemy_prefab;
    public bool debug=true;
    private Vector2 enemy_size; // enemy_Prefab to spawn
    public float spawnTime = 2.0f;  // Time interval between spawns
    public int enemy_limit = 4;  // Maximum number of objects allowed
    public List<GameObject> spawned_enemies = new List<GameObject>();  // List to track spawned objects
    void Start()
    {
        enemy_size = enemy_prefab.GetComponent<BoxCollider2D>().size;
        StartCoroutine(SpawnObjects());
    }

    // Coroutine to spawn objects at intervals
    private IEnumerator SpawnObjects()
    {

        while (true)
        {
            // Check if we can spawn a new object
            if (spawned_enemies.Count<enemy_limit )
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
    private void SpawnEnemy()
    {
        int maxAttempts = 10; // Prevent infinite loops
        int attempts = 0;
        Vector3 spawnPosition;

        while (attempts < maxAttempts)
        {
            spawnPosition = new Vector3(
                    0,-1, -1
                );
            if (spawnPosition == new Vector3(0, 0, -1))
            {
                DebugPrint("NotThisCoord");
                continue;
            }
            // Check if the position is free
            if (!Physics2D.OverlapBox(spawnPosition, enemy_size, 0))
            {
                // Spawn tombstone if no collisions
                GameObject new_enemy = Instantiate(enemy_prefab, spawnPosition, Quaternion.identity, gameObject.transform);
                new_enemy.GetComponent<EnemyData>().SetTombstoneParent(this);
                spawned_enemies.Add(new_enemy);
                
            }

            attempts++; // Increase attempts to avoid infinite loops
        }
    }


    // Call this function when an object is killed, passing the object as a parameter
    public void ObjectKilled(GameObject killedObject)
    {
        // Remove the object from the list
        if (spawned_enemies.Contains(killedObject))
        {
            spawned_enemies.Remove(killedObject);
        }

    }

    private void DebugPrint(string msg){

        if (debug){
            Debug.Log(msg);
        }
    }

}
