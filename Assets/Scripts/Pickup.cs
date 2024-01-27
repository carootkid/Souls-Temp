using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Transform mainHand;
    public Transform secondHand;

    public float pickupRadius;

    private bool switched = false;

    public float switchSpeed = 10f;

    void Start()
    {

    }

    void Update()
    {

        if (Input.GetKeyDown(playerMovement.pickup))
        {
            PickUp();
        }

        if (Input.GetKeyDown(playerMovement.switchWeapons))
        {
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

        foreach (Transform child in mainHandChildren2)
        {
            if (child.parent == mainHand.transform && child.CompareTag("Gun"))
            {
                child.GetComponent<Gun>().enabled = true;
            }
             else if(child.parent == mainHand.transform && child.CompareTag("Melee"))
            {
                child.GetComponent<Melee>().enabled = true;
            }
        }

        foreach (Transform child in secondHandChildren2)
        {
            if (child.parent == secondHand.transform && child.CompareTag("Gun"))
            {
                child.GetComponent<Gun>().enabled = false;
            }
             else if(child.parent == secondHand.transform && child.CompareTag("Melee"))
            {
                child.GetComponent<Melee>().enabled = false;
            }
        }

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
