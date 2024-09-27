using UnityEngine;

public class Rocket_Explosion : MonoBehaviour
{

    public float blastRadius;
    public float explosionForce;
    public GameObject explosionPrefab;
    public ParticleSystem smokeTrail;
    public int explosionDamage;

    private Collider[] hitColliders;

    void OnCollisionEnter(Collision col)
    {
        if (smokeTrail != null)
        {
            smokeTrail.transform.parent = null;
        }
        Explosion(col.contacts[0].point);
        GameObject explosionInstance = Instantiate(explosionPrefab, col.contacts[0].point, Quaternion.identity);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Explosion(Vector3 explosionPoint)
    {
        hitColliders = Physics.OverlapSphere(explosionPoint, blastRadius);

        foreach (Collider hitcol in hitColliders)
        {
            if (hitcol.gameObject.GetComponent<Enemy>() != null)
            {
                // Get the enemy script
                Enemy enemy = hitcol.gameObject.GetComponent<Enemy>();

                // Apply damage to the enemy
                enemy.TakeDamage(explosionDamage);
            }
            // Check if collided with the destructible building
            if (hitcol.GetComponent<DestructibleBuilding>() != null)
            {
                // Apply explosion force to the main building (optional)
                hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPoint, blastRadius, 1, ForceMode.Impulse);

                // Get all child colliders of the fractured prefab (assuming it's spawned on destruction)
                Collider[] childColliders = hitcol.gameObject.GetComponentsInChildren<Collider>();

                // Apply explosion force to each child collider with rigidbody
                foreach (Collider childCollider in childColliders)
                {
                    if (childCollider.GetComponent<Rigidbody>() != null)
                    {
                        childCollider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPoint, blastRadius, 1, ForceMode.Impulse);
                    }
                }
            }
            else
            {
                // Apply explosion force to other objects (optional)
                if (hitcol.GetComponent<Rigidbody>() != null)
                {
                    hitcol.GetComponent<Rigidbody>().isKinematic = false;
                    hitcol.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionPoint, blastRadius, 1, ForceMode.Impulse);
                }
            }
        }
    }
}
