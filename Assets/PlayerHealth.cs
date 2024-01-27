using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxPotions;
    public int currentPotions;  // Track the current number of potions the player has
    public float playerHealth = 100f;
    public float maxHealth = 100f;

    private void Start()
    {
        currentPotions = maxPotions;  // Start with the maximum number of potions
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log("Hit");

        if (playerHealth <= 0)
        {
            Debug.Log("GONE");
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
        }
    }

    void UseHealingPotion()
    {
        if (currentPotions > 0)
        {
          
            playerHealth += 20f;  
            currentPotions--;

          
            playerHealth = Mathf.Min(playerHealth, maxHealth);

            Debug.Log("Used Healing Potion. Current Health: " + playerHealth + ", Potions Left: " + currentPotions);
        }
        else
        {
            Debug.Log("No Healing Potions Left");
        }
    }
}