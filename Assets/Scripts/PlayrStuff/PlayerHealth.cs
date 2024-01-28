using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxPotions;
    public int currentPotions;
    public float playerHealth = 100f;
    public float maxHealth = 100f;
    public TextMeshProUGUI healthPotionsText;
    public Scrollbar healthScrollbar;
    public float interactRange = 5f;
    public Camera playerCamera; 
    public bool atCampfire;

    public PlayerMovement playerMovement;

    private void Start()
    {
        currentPotions = maxPotions;
        UpdateHealthPotionsText();
        UpdateHealthScrollbar();
        atCampfire = false;
    }

    public void TakeDamage(float damage)
    {
        if(playerMovement.canDamage == true){
            playerHealth -= damage;
            Debug.Log("Hit");

            if (playerHealth <= 0)
            {
                Debug.Log("GONE");
            }

            UpdateHealthScrollbar();
        }
        
    }

    private void Update()
    {
        UpdateHealthScrollbar();
        UpdateHealthPotionsText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithCampfire();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
        }
       
    }

     void InteractWithCampfire()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera not assigned.");
            return;
        }

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.green, 2f);

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Campfire"))
            {
                Debug.Log("Interacting with campfire.");

                if (!atCampfire)
                {
                    atCampfire = true; 
                    RestoreHealthPotions();
                    RestoreHealth();
                }
                else
                {
                    atCampfire = false; 
                }
            }
        }
    }

    void RestoreHealthPotions()
    {
      
        currentPotions = maxPotions;
        UpdateHealthPotionsText();
    }

    void RestoreHealth()
    {
        // Restore full health
        playerHealth = maxHealth;
        UpdateHealthScrollbar();
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
