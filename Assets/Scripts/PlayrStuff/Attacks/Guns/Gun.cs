using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public float timeToReload = 5f;
    public int currentAmmo;
    public AudioSource shootSource;
    public AudioClip shootClip;
    public bool canShoot;
    public bool hasAmmo;
    public bool reloading = false;
    public bool oneHanded = false;
    public Animator gunAnimator;

     public Scrollbar ammoScrollbar;

    private void Start()
    {
        canShoot = true;
        UpdateAmmoUI();
    }


    private void Update()
    {
        if (playerMovement.aiming)
        {
            transform.LookAt(aim);
        }
        else
        {
            transform.localEulerAngles = Vector3.zero;
        }

        if (currentAmmo > 0)
        {
            hasAmmo = true;
        }
        else
        {
            hasAmmo = false;
        }

        if (Input.GetKeyDown(playerMovement.shoot) && canShoot && hasAmmo && !reloading)
        {
            currentAmmo--;
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            gunAnimator.SetTrigger("Shoot");
            StartCoroutine(StartCooldown());
            UpdateAmmoUI();
        }
    }

    private IEnumerator StartCooldown()
    {
        canShoot = false;
        shootSource.PlayOneShot(shootClip);
        yield return new WaitForSeconds(Cooldown);
        canShoot = true;
    }

    public void Reload()
    {
        currentAmmo = ammoPerMagazine;
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoScrollbar != null)
        {
            ammoScrollbar.size = (float)currentAmmo / ammoPerMagazine;
        }
    }
}
