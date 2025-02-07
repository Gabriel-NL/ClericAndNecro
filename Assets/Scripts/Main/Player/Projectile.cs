using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed=5f;
    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyData>().DealDamage(1);
   
            Destroy(gameObject);
            return; 
        }

        if (collision.gameObject.CompareTag("MapBorder"))
        {
            Destroy(gameObject);
            return;
        }

        
    }

    void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }



}
