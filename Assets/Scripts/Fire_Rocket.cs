using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Rocket : MonoBehaviour
{

    private Animator animator;

    public GameObject rocketPrefab;
    public float propulsionForce;
    public float reloadTime = 1.0f;
    private bool canFire = true;

    public GameObject muzzleFlash;

    private Transform myTransform;

    public AudioSource audioSource1;
    public AudioSource audioSource2;

    // Start is called before the first frame update
    void Start()
    {
        SetInitialReferences();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canFire)
        {
            SpawnRocket();
            StartCoroutine(Reload()); // Start reload coroutine
        }

        if (Input.GetKeyDown(KeyCode.R) && !canFire) // Prevent reloading while firing
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && canFire)
        {
            // Play reload animation (if desired)
            animator.SetTrigger("Reload");
        }
    }

    void SpawnRocket()
    {
        // Spawn the rocket at the transform position
        GameObject rocket = (GameObject)Instantiate(rocketPrefab, myTransform.position, myTransform.rotation);
        rocket.GetComponent<Rigidbody>().AddForce(myTransform.forward * propulsionForce, ForceMode.Impulse);
        Destroy(rocket, 3);

        // Play the muzzle flash at the spawn location (with slight offset for better visibility)
        Instantiate(muzzleFlash, myTransform.position + myTransform.forward * 0.1f, myTransform.rotation);

        audioSource1.Play();
        animator.SetTrigger("Shoot");
        canFire = false; // Set canFire to false after firing
    }

    IEnumerator Reload()
    {
        // Wait until the player presses R and canFire is false
        while (!Input.GetKeyDown(KeyCode.R) || canFire)
        {
            yield return null;
        }

        // Reload after the player presses R
        audioSource2.Play();
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        canFire = true;
    }

    void SetInitialReferences()
    {
        myTransform = transform;
    }
}