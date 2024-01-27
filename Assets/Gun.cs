using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform aim;

    public PlayerMovement playerMovement;

    public Transform bulletSpawn;
    public GameObject bullet;
    public float chargeUp;
    public float Cooldown;
    public AudioSource shootSource;
    public AudioClip shootClip;
    public  bool canShoot;

    private void Start() {
        canShoot = true;
    }

    private void Update() {
        if(playerMovement.aiming){
            transform.LookAt(aim);
        } else {
            transform.localEulerAngles = Vector3.zero;
        }
        
        if(Input.GetKeyDown(playerMovement.shoot) && canShoot)
        {
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
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
