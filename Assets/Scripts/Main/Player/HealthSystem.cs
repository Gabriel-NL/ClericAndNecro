using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Transform hearts_parent;
    private Image[] hearts_array;

    public float invincibilityDuration = 1.5f; // Duration of invincibility
    public bool isInvincible = false; // Tracks if the player is invincible
    public GameOverManager gameOverManager;

    private SpriteRenderer[] all_player_sprites;

    void Start()
    {
        int childCount = hearts_parent.childCount;
        hearts_array = new Image[childCount];

        for (int i = 0; i < childCount; i++)
        {
            hearts_array[i] = hearts_parent.GetChild(i).GetChild(0).GetComponent<Image>();
        }
        currentHealth = maxHealth;
        all_player_sprites = GetComponentsInChildren<SpriteRenderer>();
        UpdateHealthUI();
    }

    private void OnCollisionEnter(Collision collided_obj)
    {
        if (collided_obj.collider.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        if (isInvincible) return; // Ignore damage if currently invincible

        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames()); // Start invincibility after getting hit
        }
    }

    IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        // Optional: Add a blinking effect to show invincibility

        while (elapsedTime < invincibilityDuration)
        {
            foreach (var sprite in all_player_sprites)
            {
                sprite.enabled = !sprite.enabled; // Toggle visibility
            }

            yield return new WaitForSeconds(0.2f); // Blink speed
            elapsedTime += 0.2f;
        }
        foreach (var sprite in all_player_sprites)
        {
            sprite.enabled = true;// Toggle visibility
        }
        isInvincible = false;
    }

    void Die()
    {
        gameOverManager.TriggerGameOver();
        Debug.Log("Player Died!");
        gameObject.SetActive(false);
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < hearts_array.Length; i++)
        {
            hearts_array[i].enabled = i < currentHealth;
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth); // Prevent overheal
        UpdateHealthUI();
    }
}