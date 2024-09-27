using UnityEngine;

public class DestructibleBuilding : MonoBehaviour
{
    public GameObject fracturedPrefab; // Reference to the fractured building prefab

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object has the "RocketProjectile" tag (replace with your actual projectile tag)
        if (collision.gameObject.tag == "Rocket")
        {
            // Instantiate the fractured model at the same position and rotation
            GameObject fracturedBuilding = Instantiate(fracturedPrefab, transform.position, transform.rotation);

            // Transfer physics velocity (optional)
            fracturedBuilding.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;

            // Destroy the intact model (optional)
            Destroy(gameObject);
        }
    }
}
