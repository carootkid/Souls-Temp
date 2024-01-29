using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeManager : MonoBehaviour
{
    public Melee melee;

    public void EnableCollider(){
        melee.EnableCollider();
    }

    public void DisableCollider(){
        melee.DisableCollider();
    }
}
