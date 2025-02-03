using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    // Start is called before the first frame update
    private int health_points = 4;
    private Tombstone tombstone_parent;
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
    public void SetTombstoneParent(Tombstone obj)
    {
        if (tombstone_parent == null)
        {
            tombstone_parent = obj;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
