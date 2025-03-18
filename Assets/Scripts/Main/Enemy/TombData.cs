using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TombData : MonoBehaviour
{
    [Header("ReadOnly variables")]
    public double extra_speed = 0;
    public LayerMask obstacleLayer; // Layer for collision checking
    [Header("Editable Variables")]
    public int tombstone_hp = 5;
    public int points_worth = 100;
    public int spawn_attempts = 5;
    public float enemy_spawn_radius = 1f;

    //Invisible variables
    private Vector3 tombstone_position;
     private Bounds bounds;

    private void Start()
    {
        tombstone_position = gameObject.transform.position;
        bounds = gameObject.GetComponent<Renderer>().bounds;

    }
    public double GetExtraSpeed()
    {
        double output = extra_speed;
        extra_speed += 0.2;
        return output;
    }
    public void DealDamage(int damage)
    {
        if (tombstone_hp > damage)
        {
            tombstone_hp -= damage;
        }
        else
        {
            FindAnyObjectByType<EnemyControl>().OnTombstoneDestroy(this);
            Destroy(gameObject);

            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(points_worth);
        }
    }

    public GameObject SpawnEnemy(GameObject enemy_prefab)
    {
        Vector3 spawnPos;
        float randomX, randomZ;
        bool isClear;
        GameObject enemy; 
        for (int i = 0; i < spawn_attempts; i++)
        {
            randomX = Random.Range(
   tombstone_position.x - enemy_spawn_radius - bounds.extents.x,
   tombstone_position.x + enemy_spawn_radius + bounds.extents.x
               );
            randomZ = Random.Range(
                tombstone_position.z - enemy_spawn_radius - bounds.extents.z,
                tombstone_position.z + enemy_spawn_radius + bounds.extents.z
            );
            spawnPos = new Vector3(randomX, tombstone_position.y, randomZ);
            // Check for obstacles
            isClear = Physics.OverlapSphere(spawnPos, enemy_spawn_radius, obstacleLayer).Length == 0;
            if (isClear)
            {

                // Instantiate the enemy and set as child of the tombstone (or other parent)
                enemy = Instantiate(enemy_prefab, spawnPos, Quaternion.identity);
                enemy.GetComponent<EnemyData>().AddExtraSpeed(GetExtraSpeed());
                return enemy;
            }
        }return null;
    }



}
