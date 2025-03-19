using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    private EnemyControl enemy_control_script;
    private Transform current_tomb;

    public float retreatSpeed = 3.5f;
    private int boss_hp = 20;
    private NavMeshAgent navMeshAgent;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player has the correct tag.");
            }
        }
    }

    public void RandomNewTombstone()
    {
        enemy_control_script=FindFirstObjectByType<EnemyControl>();
        List<Transform> all_tombstones = enemy_control_script.GetTombstoneTransforms().ToList();

        if (all_tombstones.Count > 0)
        {
            int random_index = Random.Range(0, all_tombstones.Count);
            current_tomb = all_tombstones[random_index];
            transform.SetParent(current_tomb);
            gameObject.transform.localPosition = Vector3.zero;
        }
        else
        {
            //Remove parent
            transform.SetParent(null, true);
            Debug.Log("Victory");
            enemy_control_script.KillAllEnemies();
        }
    }

    public void SetPosition(Vector3 new_pos)
    {
        gameObject.transform.localPosition = new_pos;
    }
    public Transform GetCurrentTomb()
    {
        return current_tomb;
    }

}
