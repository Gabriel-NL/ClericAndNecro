using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    // Start is called before the first frame update\

    public BreakStateController break_status_script;
    public int current_health=10;
    void Start()
    {
        
    }

    public void ObstacleDamaged(int dmg){
        if (dmg<current_health)
        {
            current_health-=dmg;
            break_status_script.NextState(1);
        }
    }
}
