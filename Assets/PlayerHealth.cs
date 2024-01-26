using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float playerHealth = 100f;
    public float maxHealth = 100f;

    public void TakeDamage(float damage){
        playerHealth -= damage;
        Debug.Log("Hit");

        if(playerHealth < 0){
            Debug.Log("GONE");
        }

    }
}
