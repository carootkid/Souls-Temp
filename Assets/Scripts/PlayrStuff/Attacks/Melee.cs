using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public bool spear = false;
    public bool light = false;
    public bool heavy = false;
    public Collider attackCollider;
    public Animator animator;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(playerMovement.shoot)){
            animator.SetTrigger("lightAttack");
        }
    }

    public void EnableCollider(){
        attackCollider.enabled = true;
        Debug.Log("Enabled Collider");
    }

    public void DisableCollider(){
        attackCollider.enabled = false;
        Debug.Log("Disabled Collider");
    }
}
