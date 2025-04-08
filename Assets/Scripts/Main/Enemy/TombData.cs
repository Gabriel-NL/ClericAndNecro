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
    public int tombstone_hp = 5,max_hp=5;
    public int points_worth = 100;
    public int spawn_attempts = 20;
    private float enemy_spawn_radius = 3;

    //Invisible variables
    private Vector3 tombstone_position;
    private BreakStateController breakStateController;

    public AudioClip tombDestroyed;


    private void Start()
    {
        tombstone_position = gameObject.transform.position;
        breakStateController = GetComponent<BreakStateController>();
        if (breakStateController == null)
        {
            Debug.LogError("BreakStateController not found");
        }

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
            breakStateController.NextState();
        }
        else
        {
            FindAnyObjectByType<EnemyControl>().OnTombstoneDestroy(transform);
            Destroy(gameObject);
            PlaySound();

            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(points_worth);
        }
    }

public void HealTomb(int heal_value){
    tombstone_hp+=heal_value;

    if (tombstone_hp >max_hp ){
        tombstone_hp = max_hp;
    }
    breakStateController.RevertDamage(heal_value);
}
    public GameObject SpawnEnemy(GameObject enemy_prefab)
    {
        Vector3 spawnPos;
        bool isClear;
        GameObject enemy;
        spawnPos = gameObject.transform.position;
        spawnPos.z -= enemy_prefab.GetComponent<Renderer>().bounds.extents.z - 5;
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

    void PlaySound()
    {
        if (tombDestroyed != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject audioObject = new GameObject("SkeletonDamage");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = tombDestroyed;
            audioSource.Play();

            // Destroy the temporary GameObject after the clip finishes playing
            Destroy(audioObject, tombDestroyed.length);
        }
    }
}
