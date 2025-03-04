using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombData : MonoBehaviour
{

    private double extra_hp=0;
    private int tombstone_hp=10;
    private int pointsWorth = 100;
    // Start is called before the first frame update
    
    public Double GetExtraHP(){
        double output=extra_hp;
        extra_hp+=0.2;
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

            FindAnyObjectByType<EnemyControl>().OnTombstoneDestroy(gameObject);
            Destroy(gameObject);

            // Add score when enemy is killed
            ScoreManager.Instance.AddScore(pointsWorth);
        }
    }
    


}
