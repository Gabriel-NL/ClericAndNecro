using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Unity.VisualScripting;

public class EnemyControl : MonoBehaviour
{
    [Header("Initial declaration")]
    public GameObject tombstone_prefab; // The tombstone prefab to spawn
    public Transform spawnParent; // Parent (SpawnPosition)
    public Collider groundCollider; // The collider defining the spawn area
    public LayerMask obstacleLayer; // Layer for collision checking
    public TMP_Text waveText;
    public TMP_Text tombstone_count;
    public GameObject boss_monster;

    [Header("MapConfigs")]
    public float margin; // Margin from collider edges
    public float checkRadius = 10f; // Collision check radius
    

    public int wave = 0;
    public int last_wave = 10;

    [Header("Tombstone Configs")]
    private List<TombData> generated_tombstones=new List<TombData>();
    private int tombstone_limit = 1;
    public float tombstone_internal_clock = 2f;

    [Header("Enemy Configs")]
    public GameObject enemy_prefab; // The tombstone prefab to spawn
    private List<GameObject> enemies_generated = new List<GameObject>();
    public float enemy_internal_clock = 2.0f;
    public int max_number_enemies = 5;
    public GameObject enemy_list_parent;
    public bool canSpawn;

    //ReadOnly Variables
    [SerializeField]
    private Vector3 min_bounds, max_bounds;

    void Start()
    {
        if (tombstone_prefab == null)
        {
            throw new System.ArgumentNullException(nameof(tombstone_prefab), "Tombstone prefab is null!");
        }

        (min_bounds, max_bounds) = CalculateSpawnBounds();
        StartCoroutine(TombstoneSpawner());
        StartCoroutine(EnemySpawner());
        wave = 1;
        UpdateWaveUI();
    }

    private (Vector3, Vector3) CalculateSpawnBounds()
    {
        // Get the collider bounds
        Bounds bounds = groundCollider.bounds;

        // Apply margin
        Vector3 min_bounds = new Vector3(bounds.min.x + margin, bounds.min.y, bounds.min.z + margin);
        Vector3 max_bounds = new Vector3(bounds.max.x - margin, bounds.max.y, bounds.max.z - margin);
        return (min_bounds, max_bounds);
    }

    private IEnumerator TombstoneSpawner()
    {
        TombData tombstone;
        while (generated_tombstones.Count < tombstone_limit)
        {
            tombstone=SpawnTombstone();
            if (tombstone!=null)
            {
                generated_tombstones.Add(tombstone);
                UpdateTombstoneCount();
            }
            
            

            yield return new WaitForSeconds(tombstone_internal_clock);
        }
    }

    private TombData SpawnTombstone()
    {
        float randomX;
        float randomZ;
        Vector3 spawnPos;
        List<Vector3> forbidden_coordinates = new List<Vector3>();
        bool isClear;
        GameObject tombstone;
        int maxAttempts = 20; // Max spawn attempts
        for (int i = 0; i < maxAttempts; i++)
        {
            // Generate a random position within bounds
            randomX = Random.Range(min_bounds.x, max_bounds.x);
            randomZ = Random.Range(min_bounds.z, max_bounds.z);
            spawnPos = new Vector3(randomX, tombstone_prefab.transform.position.y, randomZ);

            if (forbidden_coordinates.Contains(spawnPos))
            {
                continue;
            }
            else
            {
                forbidden_coordinates.Add(spawnPos);
            }
            // Check for obstacles
            isClear = Physics.OverlapSphere(spawnPos, checkRadius, obstacleLayer).Length == 0;

            if (isClear)
            {
                // Instantiate and set as child of spawnParent
                tombstone = Instantiate(tombstone_prefab, spawnPos, tombstone_prefab.transform.rotation);

                tombstone.transform.SetParent(spawnParent, worldPositionStays: true);

                return tombstone.GetComponent<TombData>();
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position.");
        return null;
    }

    public void OnTombstoneDestroy(TombData destroyed_obj)
    {

        generated_tombstones.Remove(destroyed_obj);
        UpdateTombstoneCount();
        if (generated_tombstones.Count <= 0 && wave != last_wave)
        {
            wave += 1;
            tombstone_limit = wave;
            max_number_enemies += 1;
            UpdateWaveUI();
            StartCoroutine(TombstoneSpawner());
        }
        if (wave==last_wave)
        {
            SpawnBossMonster();
        }


        Debug.Log("Victory");
    }

    private IEnumerator EnemySpawner()
    {
        int randomIndex;
        GameObject new_enemy;
        while (enemies_generated.Count != max_number_enemies)
        {

            if (enemies_generated.Count < max_number_enemies)
            {
                randomIndex = Random.Range(0, generated_tombstones.Count);
                new_enemy = generated_tombstones[randomIndex].SpawnEnemy(enemy_prefab);
                if (new_enemy != null)
                {
                    enemies_generated.Add(new_enemy);
                    new_enemy.transform.SetParent(enemy_list_parent.transform, true);
                }
                else
                {
                    continue;
                }
            }
            yield return new WaitForSeconds(5);
        }
    }

    public void OnEnemyDestroy(GameObject destroyed_obj)
    {
        enemies_generated.Remove(destroyed_obj);
    }

    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = "Current wave: " + wave;
        }
    }

    public void UpdateTombstoneCount()
    {
        tombstone_count.text = "Tombstones Remaining: " + generated_tombstones.Count();
    }
    public void SpawnBossMonster()
    {
        Instantiate(boss_monster, spawnParent);
    }

}
