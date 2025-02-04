using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed=5f;
    // This function is automatically called when the projectile collides with something
    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyData>().DealDamage(1);
   
            Destroy(gameObject);
            return; 
        }
        // Check if the object the projectile collides with has the "MapBorder" tag
        if (collision.gameObject.CompareTag("MapBorder"))
        {
           
            // Destroy the projectile upon collision with MapBorder
            Destroy(gameObject);
            return;
        }

        
    }

    void FixedUpdate()
    {
        // Move the projectile forward in its local direction
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }



}
