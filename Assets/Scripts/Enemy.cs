using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Method to heal the object (not used in this example)
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        // Disable the animator component in the parent object (optional)
        transform.parent.GetComponent<Animator>().enabled = false;

        // Disable mesh collider and other components (as needed)
        GetComponent<MeshCollider>().enabled = false;

        // Disable the Enemy_AI script on this object
        Enemy_AI enemyAI = transform.parent.GetComponent<Enemy_AI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false; // Disable the Enemy_AI script
        }

        transform.parent.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GameObject waveSpawner = GameObject.Find("Wave Spawner");
        if (waveSpawner != null) // Check if Wave Spawner exists before accessing its component
        {
            waveSpawner.GetComponent<Wave_Spawner>().EnemyDefeated();
        }
    }
}
