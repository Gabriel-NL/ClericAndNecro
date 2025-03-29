using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : MonoBehaviour
{
    public int healAmount = 1; // Amount of health restored
    public AudioClip collectSound; // The sound effect when collected

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem playerHealth = collision.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                PlaySound(); // Play the sound
                Destroy(gameObject); // Destroy the healing item
            }
        }
    }

    void PlaySound()
    {
        if (collectSound != null)
        {
            // Create a temporary GameObject to play the sound
            GameObject audioObject = new GameObject("HealingSound");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = collectSound;
            audioSource.Play();

            // Destroy the temporary GameObject after the clip finishes playing
            Destroy(audioObject, collectSound.length);
        }
    }
}
