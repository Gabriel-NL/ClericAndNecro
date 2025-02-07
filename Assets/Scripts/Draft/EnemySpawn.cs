using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    public GameObject enemy_prefab;
    public bool debug=true;
    private Vector2 enemy_size;
    public float spawnTime = 2.0f; 
    public int enemy_limit = 4; 
    public List<GameObject> spawned_enemies = new List<GameObject>();  
    void Start()
    {
        enemy_size = enemy_prefab.GetComponent<BoxCollider2D>().size;
        StartCoroutine(SpawnObjects());
    }


    private IEnumerator SpawnObjects()
    {

        while (true)
        {
            if (spawned_enemies.Count<enemy_limit )
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
    private void SpawnEnemy()
    {
        int maxAttempts = 10;
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

            if (!Physics2D.OverlapBox(spawnPosition, enemy_size, 0))
            {

                GameObject new_enemy = Instantiate(enemy_prefab, spawnPosition, Quaternion.identity, gameObject.transform);
                //new_enemy.GetComponent<EnemyData>().SetTombstoneParent(this);
                spawned_enemies.Add(new_enemy);
                
            }

            attempts++; 
        }
    }


    public void ObjectKilled(GameObject killedObject)
    {
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
