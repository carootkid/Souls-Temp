using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Transform mainHand;
    public Transform secondHand;
    public Transform aimTransform;

    public float pickupRadius;

    public Animator handAnimator;

    private bool canSwitch = false;
    private bool reloading;
    private bool switchCooldown = false;
    private float switchCooldownAmount = 1.0f;

    public PlayerHealth playerHealth;

    public AmmoManager ammoManager;

    private float timeToReload = 0f;

    public bool foundGunOrMelee;

    private Gun gunScript;

    void Update()
    {

        if (Input.GetKeyDown(playerMovement.pickup))
        {
            PickUp();
        }

        
        bool ammoNotMaxed = true;

        if(gunScript != null)
        {

            if(gunScript.currentAmmo == gunScript.ammoPerMagazine){
                ammoNotMaxed = false;
                Debug.Log("Ammo Is Maxed.");
            } else {
                ammoNotMaxed = true;
                
                Debug.Log("Ammo Not Maxed.");
            }
        }
        
        

        if(Input.GetKeyDown(playerMovement.reload) && foundGunOrMelee && ammoManager.ammoAmount > 0 && ammoNotMaxed)
        {
            StartCoroutine(Reload());
            Debug.Log("Reloading...");
        }

        if (Input.GetKeyDown(playerMovement.switchWeapons) && canSwitch && !reloading)
        {
            handAnimator.SetTrigger("Switch");

            handAnimator.SetBool("idle", true);
            handAnimator.SetBool("OneHanded", false);
            handAnimator.SetBool("light", false);
            handAnimator.SetBool("heavy", false);
            handAnimator.SetBool("spear", false);
            Transform[] mainHandChildren = mainHand.GetComponentsInChildren<Transform>(true);
            Transform[] secondHandChildren = secondHand.GetComponentsInChildren<Transform>(true);

            foreach (Transform child in mainHandChildren)
            {
                if (child.parent == mainHand.transform)
                {
                    child.parent = secondHand.transform;
                    child.localPosition = Vector3.zero;
                    child.localEulerAngles = Vector3.zero;
                }
            }

            foreach (Transform child in secondHandChildren)
            {
                if (child.parent == secondHand.transform)
                {
                    child.parent = mainHand.transform;
                    child.localPosition = Vector3.zero;
                    child.localEulerAngles = Vector3.zero;
                }
            }

            StartCoroutine(SwitchCooldown());
        }

        Transform[] mainHandChildren2 = mainHand.GetComponentsInChildren<Transform>(true);
        Transform[] secondHandChildren2 = secondHand.GetComponentsInChildren<Transform>(true);

        foundGunOrMelee = false;

        
        foreach (Transform child in mainHandChildren2)
        {
            if (child.parent == mainHand.transform)
            {
                if (child.CompareTag("Gun"))
                {
                    gunScript = child.GetComponent<Gun>();
                    if (gunScript != null)
                    {
                        gunScript.enabled = true;
                        if (gunScript.oneHanded)
                        {
                            handAnimator.SetBool("OneHanded", true);
                            handAnimator.SetBool("isGun", true);
                            handAnimator.SetBool("idle", false);
                        }
                        else
                        {
                            handAnimator.SetBool("OneHanded", false);
                            handAnimator.SetBool("isGun", true);
                            handAnimator.SetBool("idle", false);
                        }
                        foundGunOrMelee = true;
                    }
                }
                else if (child.CompareTag("Melee"))
                {
                    Melee meleeScript = child.GetComponent<Melee>();

                    meleeScript.enabled = true;

                    if(meleeScript != null){
                        if(meleeScript.spear){
                            handAnimator.SetBool("isGun", false);
                            handAnimator.SetBool("spear", true);
                            handAnimator.SetBool("idle", false);
                        } else if(meleeScript.light){
                            handAnimator.SetBool("isGun", false);
                            handAnimator.SetBool("light", true);
                            handAnimator.SetBool("idle", false);
                        } else if(meleeScript.heavy){
                            handAnimator.SetBool("isGun", false);
                            handAnimator.SetBool("heavy", true);
                            handAnimator.SetBool("idle", false);
                        }
                        
                        foundGunOrMelee = true;
                    }
                }
            }
        }

        foreach (Transform child in secondHandChildren2)
        {
            if (child.parent == secondHand.transform && child.CompareTag("Gun"))
            {
                child.GetComponent<Gun>().enabled = false;
            }
            else if (child.parent == secondHand.transform && child.CompareTag("Melee"))
            {
                child.GetComponent<Melee>().enabled = false;
            }
        }

        if (!foundGunOrMelee)
        {
            handAnimator.SetBool("idle", true);
        }

        


        canSwitch = !switchCooldown;

    }

    public void PickUp()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Gun") || collider.CompareTag("Melee"))
            {
                
                

                if (mainHand.childCount > 0)
                {
                    Debug.Log("Main hand is FULL");
                    return;
                }

                Debug.Log("Got " + collider.tag);
                collider.transform.parent = mainHand;
                collider.transform.localPosition = Vector3.zero;
                collider.transform.localEulerAngles = Vector3.zero;
                collider.transform.localScale = new Vector3(1f,1f,1f);
                Rigidbody colliderRigidbody = collider.GetComponent<Rigidbody>();
                
                Collider colliderCollider = collider.GetComponent<Collider>();
                if (colliderRigidbody != null)
                    colliderRigidbody.isKinematic = true;

                if (colliderCollider != null)
                    colliderCollider.enabled = false;

                    if(collider.CompareTag("Gun"))
                    {
                        collider.GetComponent<Gun>().playerMovement = playerMovement;
                        collider.GetComponent<Gun>().aim = aimTransform;
                        collider.GetComponent<Gun>().ammoManager = ammoManager;

                        timeToReload = collider.GetComponent<Gun>().timeToReload;
                    } else {
                        Melee meleeScript = collider.GetComponent<Melee>();
                        meleeScript.playerMovement = playerMovement;
                        meleeScript.animator = handAnimator;
                    }
            }
            else if (collider.CompareTag("Pickup"))
            {
                Debug.Log("Got Pickup");
                PickupType type = collider.GetComponent<PickupType>();

                if(type.isAmmo != true){
                    if(playerHealth.currentPotions != playerHealth.maxPotions)
                    {
                        playerHealth.currentPotions += type.amount;
                        Debug.Log(type.amount);
                        type.DestroySelf();
                    }
                    else
                    {
                        Debug.Log("Max Potions Reached.");
                    }
                    
                    
                } else {
                    if(ammoManager.ammoAmount != ammoManager.maxAmmo){
                        ammoManager.ammoAmount += type.amount;
                        Debug.Log(type.amount);
                        type.DestroySelf();
                    }
                    else
                    {
                        Debug.Log("Max Ammo Reached.");
                    }
                }

                
            } else if(collider.CompareTag("Armor"))
            {
                Debug.Log("Got armor");
            }
        }

    }

    IEnumerator SwitchCooldown()
    {
        switchCooldown = true;

        yield return new WaitForSeconds(switchCooldownAmount);

        switchCooldown = false;
    }

    IEnumerator Reload()
    {
        reloading = true;

        gunScript.reloading = true;

        yield return new WaitForSeconds(timeToReload);

        ammoManager.ammoAmount--;

        gunScript.Reload();

        gunScript.reloading = false;

        reloading = false;
        Debug.Log("Reloaded.");
        
    }
}
