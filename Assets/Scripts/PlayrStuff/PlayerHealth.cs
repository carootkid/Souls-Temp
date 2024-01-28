using System.Collections;
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
    public float interactionRange = 5f;
    public PlayerMovement playerMovement;

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
            Debug.Log("GONE");
        }

        UpdateHealthScrollbar();
    }

    private void Update()
    {
        UpdateHealthScrollbar();
        UpdateHealthPotionsText();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCampInteract();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseHealingPotion();
            UpdateHealthScrollbar();
        }
    }

      void TryCampInteract()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
            {
               
                Debug.DrawRay(transform.position, transform.forward * interactionRange, Color.blue, 1f);
                Debug.Log("shot");
                if (hit.collider.CompareTag("Campfire"))
                {
                    campInteract();
                }
            }
    }

    public void campInteract()
    {
        currentPotions = maxPotions;
        playerHealth = maxHealth;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        StartCoroutine(EnablePlayerMovementAfterDelay(5f));
    }

    IEnumerator EnablePlayerMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
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
