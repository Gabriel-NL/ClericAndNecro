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
    public int spawn_attempts = 20;
    private float enemy_spawn_radius = 3;

    //Invisible variables
    private Vector3 tombstone_position;


    private void Start()
    {
        tombstone_position = gameObject.transform.position;

    }
    public double GetExtraSpeed()
    {
        double output = extra_speed;
        extra_speed += 0.1;
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
            FindAnyObjectByType<EnemyControl>().OnTombstoneDestroy(transform);
            Destroy(gameObject);

            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(points_worth);
        }
    }

    public GameObject SpawnEnemy(GameObject enemy_prefab)
    {
        Vector3 spawnPos;
        bool isClear;
        GameObject enemy;
        spawnPos = gameObject.transform.position;
        spawnPos.z += enemy_prefab.GetComponent<Renderer>().bounds.extents.z + 5;
        isClear = Physics.OverlapSphere(spawnPos, enemy_spawn_radius, obstacleLayer).Length == 0;
        if (isClear)
        {
            // Instantiate the enemy and set as child of the tombstone (or other parent)
            enemy = Instantiate(enemy_prefab, spawnPos, enemy_prefab.transform.rotation);
            enemy.GetComponent<EnemyData>().AddExtraSpeed(GetExtraSpeed());
            return enemy;
        }
        else
        {
            return null;
        }
    }
}
