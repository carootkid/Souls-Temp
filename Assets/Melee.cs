using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public bool spear = false;
    public bool swordAxeLight = false;
    public bool swordAxeHeavy = false;
    public bool blunt = false;
    public Collider attackCollider;

    public Animator meleeAnimator;

    void Start()
    {
        if(spear)
        {
            meleeAnimator.SetBool("spear", true);
        } else if(swordAxeLight)
        {
            
        } else if(swordAxeHeavy)
        {
            
        } else if(blunt)
        {
            
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(spear)
        {
            Debug.Log("SPEAR");
            if(Input.GetKeyDown(playerMovement.shoot))
            {
                meleeAnimator.SetBool("spearCharge", true);
                Debug.Log("Charging..");
            } 
            else if(Input.GetKeyUp(playerMovement.shoot))
            {
                meleeAnimator.SetBool("spearCharge", false);
                meleeAnimator.SetTrigger("SpearAttack");
                Debug.Log("attacked");
            }
        } else if(swordAxeLight)
        {
            Debug.Log("LIGHT SWORD/AXE");
        } else if(swordAxeHeavy)
        {
            Debug.Log("HEAVY SWORD/AXE");
        } else if(blunt)
        {
            Debug.Log("BLUNT");
        } 
    }

    public void EnableCollider(){
        attackCollider.enabled = true;
    }

    public void DisableCollider(){
        attackCollider.enabled = false;
    }
}
