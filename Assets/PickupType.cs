using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupType : MonoBehaviour
{
    public bool isAmmo = false;

    public int amount = 1;

    public void DestroySelf(){
        Destroy(gameObject);
    }
}
