using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Transform hearts_parent;
    public Image[] hearts_array;
    
    public float invincibilityDuration = 1.5f; // Duration of invincibility
    private bool isInvincible = false; // Tracks if the player is invincible
    public GameOverManager gameOverManager;

    void Start()
    {
        int childCount = hearts_parent.childCount;
        hearts_array = new Image[childCount];

        for (int i = 0; i < childCount; i++)
        {
            hearts_array[i] = hearts_parent.GetChild(i).GetChild(0).GetComponent<Image>();
        }
        currentHealth = maxHealth;
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
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        while (elapsedTime < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled; // Toggle visibility
            }
            yield return new WaitForSeconds(0.2f); // Blink speed
            elapsedTime += 0.2f;
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true; // Ensure visibility after invincibility ends
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