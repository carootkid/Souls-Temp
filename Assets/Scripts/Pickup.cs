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

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(playerMovement.pickup))
        {
            PickUp();
        }

        if (Input.GetKeyDown(playerMovement.switchWeapons) && canSwitch)
        {
            handAnimator.SetTrigger("Switch");
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
        }

        Transform[] mainHandChildren2 = mainHand.GetComponentsInChildren<Transform>(true);
        Transform[] secondHandChildren2 = secondHand.GetComponentsInChildren<Transform>(true);

        bool foundGunOrMelee = false;
        bool mainHandHasGun = false;
        bool secondHandHasGun = false;

        foreach (Transform child in mainHandChildren2)
        {
            if (child.parent == mainHand.transform)
            {
                if (child.CompareTag("Gun"))
                {
                    Gun gunScript = child.GetComponent<Gun>();
                    if (gunScript != null)
                    {
                        gunScript.enabled = true;
                        if (gunScript.oneHanded)
                        {
                            handAnimator.SetBool("OneHanded", true);
                            handAnimator.SetBool("idle", false);
                        }
                        else
                        {
                            handAnimator.SetBool("OneHanded", false);
                            handAnimator.SetBool("idle", false);
                        }
                        foundGunOrMelee = true;
                        mainHandHasGun = true;
                    }
                }
                else if (child.CompareTag("Melee"))
                {
                    Melee meleeScript = child.GetComponent<Melee>();
                    if (meleeScript != null)
                    {
                        meleeScript.enabled = true;
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
                secondHandHasGun = true;
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

        canSwitch = mainHandHasGun || secondHandHasGun;


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
                    } else {
                        collider.GetComponent<Melee>().playerMovement = playerMovement;
                    }
            }
            else if (collider.CompareTag("Pickup"))
            {
                Debug.Log("Got Pickup");
            }
        }

    }
}
