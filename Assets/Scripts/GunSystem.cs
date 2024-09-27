using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{

    //gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    public Transform PlayerTransform;


    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject bulletHoleGraphicUntagged, bulletHoleGraphicEnemy, muzzleFlash; // References for different bullet hole graphics
    private Animator animator;
    public bool isShooting;

    //Audio
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    


    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        animator = GetComponentInParent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on FPS arms!");
        }
    }


    private void Update()
    {
        MyInput();
    }


    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(xSpread, ySpread, 0);

        if (Physics.Raycast(fpsCam.transform.position, direction, out RaycastHit rayHit, range))
        {
            if (rayHit.collider != null)
            {
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                }

                // Determine the bullet hole graphic based on the target's tag
                GameObject bulletHoleGraphic = rayHit.collider.CompareTag("Enemy") ? bulletHoleGraphicEnemy : bulletHoleGraphicUntagged;

                // Get the rotation that aligns the graphic with the hit surface normal
                Quaternion rotation = Quaternion.LookRotation(rayHit.normal);

                // Rotate 180 degrees around the Y-axis to face the player
                //rotation *= Quaternion.Euler(0, 0, 0);

                // Spawn the bullet hole effect at the hit point with the calculated rotation
                Instantiate(bulletHoleGraphic, rayHit.point, rotation);
            }
        }

        GameObject muzzleFlashInstance = Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
        muzzleFlashInstance.transform.parent = attackPoint;

        bulletsLeft -= bulletsPerTap;
        bulletsShot--;

        animator.SetTrigger("Shoot");
        animator.SetBool("IsShooting", true);

        audioSource1.Play(); // Play the gunshot audio clip

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        animator.SetBool("IsShooting", false);  // Set IsShooting to false
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        animator.SetTrigger("Reload");
        audioSource2.Play();
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
