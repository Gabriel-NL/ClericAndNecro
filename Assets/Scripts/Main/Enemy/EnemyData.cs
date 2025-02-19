using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    private int health_points = 4;
    public int pointsWorth = 10;
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

            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(pointsWorth);

        }
    }

}
