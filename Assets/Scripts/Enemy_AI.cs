using UnityEngine;
using UnityEngine.AI;

public class Enemy_AI : MonoBehaviour
{
    [SerializeField] Transform target;
    NavMeshAgent navMeshAgent;
    Animator animator;
    public float attackDistance = 1.5f; // Adjust this value based on your attack range
    public int damageAmount = 10;
    float attackTimer = 0f; // Timer to track attack cooldown
    public float attackCooldown = 1f; // Time between attacks (adjust based on animation)
    

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // If target is not assigned in the Inspector, find the player GameObject and assign its transform
        if (target == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                target = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("Player object not found!");
            }
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Ensure target is assigned before setting the destination
        if (target != null)
        {
            navMeshAgent.SetDestination(target.position);

            float distanceToPlayer = Vector3.Distance(transform.position, target.position); // Corrected line

            // Attack when in range
            if (distanceToPlayer <= attackDistance && attackTimer <= 0f)
            {
                attackTimer = attackCooldown;
                navMeshAgent.isStopped = true;
                animator.SetTrigger("Attack");
                AttackPlayer(target.gameObject);
            }
        
            if (attackTimer > 0f) // Decrement timer if attack is on cooldown
            {
                attackTimer -= Time.deltaTime;
            }

            // Check if animation finished and attack timer depleted
            if (!animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && attackTimer <= 0f)
            {
                // Resume agent movement
                navMeshAgent.isStopped = false;
            }
        }
    }

    public void AttackPlayer(GameObject player) // Define the method here
    {
        // Get the player health script
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Deal damage to the player
            playerHealth.TakeDamage(damageAmount);
        }
    }
}