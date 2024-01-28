using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform aim;

    public PlayerMovement playerMovement;

    public AmmoManager ammoManager;

    public Transform bulletSpawn;
    public GameObject bullet;
    public float chargeUp;
    public float Cooldown;
    public int ammoPerMagazine = 30;
    public int currentAmmo;
    public AudioSource shootSource;
    public AudioClip shootClip;
    public  bool canShoot;
    public bool hasAmmo;

    public bool oneHanded = false;

    public Animator gunAnimator;

    private void Start() {
        canShoot = true;
    }

    private void Update() {
        if(playerMovement.aiming){
            transform.LookAt(aim);
        } else {
            transform.localEulerAngles = Vector3.zero;
        }

        hasAmmo = currentAmmo > 0;
        
        if(Input.GetKeyDown(playerMovement.shoot) && canShoot && hasAmmo)
        {
            currentAmmo--;
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            gunAnimator.SetTrigger("Shoot");
            StartCoroutine(StartCooldown());
        }
    }

    private IEnumerator StartCooldown()
    {
        canShoot = false;
        shootSource.PlayOneShot(shootClip);
        yield return new WaitForSeconds(Cooldown);
        canShoot = true;
    }
}
