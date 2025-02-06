using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 5f;

    void OnTriggerEnter(Collider target_hit)
    {
        if (target_hit.gameObject.CompareTag("Enemy"))
        {
            target_hit.gameObject.GetComponent<EnemyData>().DealDamage(1);

            Destroy(gameObject);
            return;
        }
        // Check if the object the projectile collides with has the "MapBorder" tag
        if (target_hit.gameObject.CompareTag("MapBorder"))
        {

            // Destroy the projectile upon collision with MapBorder
            Destroy(gameObject);
            return;
        }
    }
    void FixedUpdate()
    {
        // Move the projectile forward in its local direction
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }



}
