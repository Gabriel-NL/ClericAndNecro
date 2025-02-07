using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int health_points = 4;
    private EnemySpawn tombstone_parent;
    void Start()
    {

    }

    public void DealDamage(int damage)
    {
        if (health_points > damage)
        {
            health_points -= damage;
        }
        else
        {
            tombstone_parent.ObjectKilled(gameObject);
            Destroy(gameObject);
        }
    }
    public void SetTombstoneParent(EnemySpawn obj)
    {
        if (tombstone_parent == null)
        {
            tombstone_parent = obj;

        }
    }

    void Update()
    {

    }
}
