using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 

public class PlayerHealth : MonoBehaviour
{
    public bool CampfireInteraction;
    public int maxPotions;
    public int currentPotions;  
    public float playerHealth = 100f;
    public float maxHealth = 100f;
    public TextMeshProUGUI healthPotionsText; 
    public Scrollbar healthScrollbar; 

    private void Start()
    {
        currentPotions = maxPotions;  
        UpdateHealthPotionsText();
        UpdateHealthScrollbar();
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        Debug.Log("Hit");

        if (playerHealth <= 0)
        {
            Debug.Log("GONE");
        }

        UpdateHealthScrollbar(); 
    }

   
    private void Update()
    {
        UpdateHealthScrollbar(); 
        UpdateHealthPotionsText();

        if(Input.GetKeyDown(KeyCode.E)){
            campInteract();
            Debug.Log("used campfire");
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
            
            UpdateHealthScrollbar(); 
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

    public void campInteract() {
        currentPotions = 3;
        playerHealth = 100;
    }
    

    void UpdateHealthPotionsText()
    {
        if (healthPotionsText != null)
        {
            healthPotionsText.text = "" + currentPotions;
        }
    }

    void UpdateHealthScrollbar()
    {
        if (healthScrollbar != null)
        {
            
            float healthPercentage = playerHealth / maxHealth;
            healthScrollbar.size = healthPercentage;
        }
    }
}