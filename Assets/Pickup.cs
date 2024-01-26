using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerMovement playerMovement;
    
    public Transform mainHand;
    public Transform secondHand;

    public float pickupRadius;

    private Transform location1;
    private Transform location2;

    private bool switched = false;


    void Start()
    {
        location1 = mainHand;
        location2 = secondHand;
    }

    void Update()
    {
        
        if(Input.GetKeyDown(playerMovement.pickup)){
            PickUp();
        }

        if(Input.GetKeyDown(playerMovement.switchWeapons)){
            switched = !switched;
            if(switched){
                mainHand.position = location1.position;
                mainHand.rotation = location1.rotation;

                secondHand.position = location2.position;
                secondHand.rotation = location2.rotation;
            } else {
                mainHand.position = location2.position;
                mainHand.rotation = location2.rotation;

                secondHand.position = location1.position;
                secondHand.rotation = location1.rotation;
            }
        }
    }

    public void PickUp(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Gun"))
            {
                Debug.Log("Got Gun");
            } 
            else if(collider.CompareTag("Melee"))
            {
                Debug.Log("Got Melee");
            } 
            else if(collider.CompareTag("Pickup"))
            {
                Debug.Log("Got Pickup");
            }
        }
    }
}
