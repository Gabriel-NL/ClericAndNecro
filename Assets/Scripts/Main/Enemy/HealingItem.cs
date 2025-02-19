using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealingItem : MonoBehaviour
{
    public int healAmount = 1; // How much health is restored

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject); // Remove the item after healing
            }
        }
    }
}
