using UnityEngine;
using UnityEngine.UI;

public class CampfireScript : MonoBehaviour
{
    public GameObject restUI;
    public GameObject playerObject; 
    private PlayerHealth playerHealth;
    public GameObject baseUI;
    private LevelUpScript levelUpScript;
    public int healthlevel = 1;
    public int ammolevel = 1;
    public int staminalevel = 1;
    public int strengthlevel = 1;

    private void Start()
    {
        restUI.SetActive(false);
        FindPlayerHealth();
    }

    private void FindPlayerHealth()
    {
        if (playerObject != null)
        {
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

    public void increaseHealth()
    {
        if(levelUpScript.levelPoints >= 1 && healthlevel <= 50)
        {
            healthlevel++;
            Debug.Log("leveled health");
        }
    }
    public void increaseAmmo()
    {
        if(levelUpScript.levelPoints >= 1 && ammolevel <= 50)
        {
            ammolevel++;
            Debug.Log("leveled Ammo");
        }
    }
    public void increaseStength()
    {
        if(levelUpScript.levelPoints >= 1 && strengthlevel <= 50)
        {
            strengthlevel++;
            Debug.Log("leveled strength");
        }
    }
    public void increaseStamina()
    {
        if(levelUpScript.levelPoints >= 1 && staminalevel <= 50)
        {
            staminalevel++;
            Debug.Log("leveled stamina");
        }
    }
}
