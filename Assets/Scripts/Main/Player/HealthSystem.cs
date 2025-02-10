using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public Transform hearts_parent;
    public Image[] hearts_array; // Assign heart images in the Inspector

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
    private void OnCollisionEnter(Collision collided_obj) {
        if (collided_obj.collider.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        gameObject.SetActive(false);
    }

    void UpdateHealthUI()
    {
        // Loop through hearts and enable/disable them based on health
        for (int i = 0; i < hearts_array.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts_array[i].enabled = true; // Show heart
            }
            else
            {
                hearts_array[i].enabled = false; // Hide heart
            }
        }
    }
}
