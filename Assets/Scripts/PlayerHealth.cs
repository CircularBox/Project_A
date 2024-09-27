using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Vignette_Take_Damage vignetteEffect;

    public Health_Bar healthbar;

    public AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) // Check health before damage reduction
        {
            Die();
            return; // Exit the method if player is already dead
        }

        Debug.Log("Damage Taken");
        audioSource.Play();
        currentHealth -= damageAmount;
        healthbar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        else
        {
            StartCoroutine(vignetteEffect.TakeDamageEffect());
        }
    }

    private void Die()
    {
        // Implement player death logic here (e.g., play animation, disable movement, display game over screen)
        //Debug.Log("Player Died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}