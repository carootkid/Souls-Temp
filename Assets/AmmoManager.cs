using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public int ammoAmount = 100;
    public int maxAmmo = 100;
    private int ammoStat;

    public TextMeshProUGUI ammoText;

    public LevelUpScript levelUpScript;
    public CampfireScript campfireScript;
    private void Update()
    {
        ammoText.text = "" + ammoAmount;

        if(ammoAmount > maxAmmo)
        {
            ammoAmount = maxAmmo;
        }
    }
    public void ammoLevel()
    {
        if (campfireScript != null)
        {
            ammoStat = campfireScript.ammolevel;

            if (ammoStat >= 0 && levelUpScript.levelPoints > 0)
            {
                maxAmmo = maxAmmo + ammoStat * 1;
                ammoAmount = maxAmmo;
                Debug.Log("Health increased. New Max Health: " + maxAmmo);
            }
            else
            {
                Debug.LogError("Invalid ammo stat value in CampfireScript: " + ammoStat);
            }
        }
        else
        {
            Debug.LogError("campfireScript is not assigned.");
        }
    }
}
