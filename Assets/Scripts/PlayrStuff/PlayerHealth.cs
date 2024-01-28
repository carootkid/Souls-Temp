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
    public PlayerMovement playerMovement;
    public LayerMask campfireLayer;

    private void Start()
    {
        currentPotions = maxPotions;
        UpdateHealthPotionsText();
        UpdateHealthScrollbar();
    }

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;

        if (playerHealth <= 0)
        {
            
        }

        UpdateHealthScrollbar();
    }

    private void Update()
    {
        UpdateHealthScrollbar();
        UpdateHealthPotionsText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCampfireInteract();
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
        }
        else
        {
            
        }
    }

    void TryCampfireInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, campfireLayer))
        {
            if (hit.collider.CompareTag("Campfire"))
            {
                CampfireInteract();
            }
        }
    }

    public void CampfireInteract()
    {
        currentPotions = maxPotions;
        playerHealth = maxHealth;
        playerMovement.setActive = false;

        UpdateHealthPotionsText();
        UpdateHealthScrollbar();
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
