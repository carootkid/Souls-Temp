using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform aim;

    public PlayerMovement playerMovement;

    private void Update() {
        if(playerMovement.aiming){
            transform.LookAt(aim);
        } else {
            transform.localEulerAngles = Vector3.zero;
        }
        
    }
}
