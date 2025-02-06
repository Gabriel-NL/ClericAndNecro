using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    // Start is called before the first frame update
    private int health_points = 4;
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
            Destroy(gameObject);
        }
    }



}
