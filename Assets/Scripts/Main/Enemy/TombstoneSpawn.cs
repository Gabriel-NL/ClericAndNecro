using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneSpawn : MonoBehaviour
{
    public GameObject tombstone_prefab;
    public bool debug=true;
    private float range = 8;
    private Vector2 tombstone_size;
    public float spawnTime = 2.0f;  // Time interval between spawns
    public int max_tombstones = 4;  // Maximum number of objects allowed
    private List<GameObject> spawned_tombstones = new List<GameObject>();  // List to track spawned objects



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
            if (spawned_tombstones.Count<max_tombstones)
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
        Vector3 spawnPosition;

        while (attempts < maxAttempts)
        {
            // Pick a random position within the defined area
            spawnPosition = new Vector3(
                Random.Range(-range, range),
                Random.Range(-range, range), -1
            );

            // Check if the position is free
            if (!Physics2D.OverlapBox(spawnPosition, tombstone_size*3, 0))
            {
                // Spawn tombstone if no collisions
                GameObject new_tombstone=Instantiate(tombstone_prefab, spawnPosition, Quaternion.identity);
                spawned_tombstones.Add(new_tombstone);
                return;
            }



            attempts++; // Try again with a new position
        }

        Debug.LogWarning("Failed to find a valid spawn position for tombstone.");
    }

    private void DebugPrint(string msg){

        if (debug){
            Debug.Log(msg);
        }
    }
}
