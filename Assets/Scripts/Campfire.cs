using UnityEngine;
using UnityEngine.UI;

public class CampfireScript : MonoBehaviour
{
    public GameObject restUI;
    public GameObject playerObject; // Drag the player object into this field in the Unity Editor
    private PlayerHealth playerHealth;
    public GameObject baseUI;

    private void Start()
    {
        restUI.SetActive(false);
        FindPlayerHealth();
    }

    private void FindPlayerHealth()
    {
        if (playerObject != null)
        {
            // Get the PlayerHealth component from the playerObject
            playerHealth = playerObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.atCampfire = false;
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on the playerObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not assigned in the Unity Editor!");
        }
    }

    private void Update()
    {
        ToggleRest();

        if(playerHealth.atCampfire)
        {
            baseUI.SetActive(false);
        }
        else
        {
            baseUI.SetActive(true);
        }
    }

    void ToggleRest()
    {
        if (playerHealth != null)
        {
            if (playerHealth.atCampfire)
            {
                ShowRestUI();
            }
            else
            {
                HideRestUI();
            }
        }
        else
        {
            Debug.LogError("PlayerHealth component not found!");
        }
    }

    void ShowRestUI()
    {
        restUI.SetActive(true);
        Debug.Log("Player entered rest mode at the campfire.");
    }

    void HideRestUI()
    {
        restUI.SetActive(false);
        
    }
}
