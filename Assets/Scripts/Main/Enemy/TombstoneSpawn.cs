using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneSpawn : MonoBehaviour
{
    public GameObject tombstone_prefab;
    public bool debug=true;
    private float range = 8;
    private Vector2 tombstone_size;
    public float spawnTime = 2.0f;
    public int max_tombstones = 4;
    private List<GameObject> spawned_tombstones = new List<GameObject>();



    void Start()
    {
        tombstone_size = tombstone_prefab.GetComponent<BoxCollider2D>().size;
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {

        while (true)
        {
            if (spawned_tombstones.Count<max_tombstones)
            {
                SpawnTombstone();
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }




    void SpawnTombstone()
    {
        int maxAttempts = 10;
        int attempts = 0;
        Vector3 spawnPosition;

        while (attempts < maxAttempts)
        {
            spawnPosition = new Vector3(
                Random.Range(-range, range),
                Random.Range(-range, range), -1
            );

            if (!Physics2D.OverlapBox(spawnPosition, tombstone_size*3, 0))
            {
                GameObject new_tombstone=Instantiate(tombstone_prefab, spawnPosition, Quaternion.identity);
                spawned_tombstones.Add(new_tombstone);
                return;
            }



            attempts++;
        }

        Debug.LogWarning("Failed to find a valid spawn position for tombstone.");
    }

    private void DebugPrint(string msg){

        if (debug){
            Debug.Log(msg);
        }
    }
}
