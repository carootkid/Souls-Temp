using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHider : MonoBehaviour
{
    private PlayerHealth playerHealth; 
    void Start()
    {
        
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found on the GameObject with the tag 'Player'.");
        }

        HideMouse();
    }

    void Update()
    {
        CampfireCheck();
    }

    private void CampfireCheck()
    {
        if ( playerHealth.atCampfire)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void HideMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowMouse()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
