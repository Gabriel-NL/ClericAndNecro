using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    public int damage=1;

    void OnTriggerEnter(Collider target_hit)
   {
        string target_tag=target_hit.gameObject.tag;
       
        switch (target_tag)
        {
            case "Enemy":
            target_hit.gameObject.GetComponent<EnemyData>().DealDamage(damage);
            break;

            case "Tomb":
            target_hit.gameObject.GetComponent<TombData>().DealDamage(damage);
            break;

            case "MapBorder":
            break;

            case "PileBones":
            target_hit.gameObject.GetComponent<BreakStateController>().NextState();
            break;


            default:
            break;
        }
        Destroy(gameObject);
    }
    void Update()
    {
        // Move the projectile forward in its local direction
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }



}
