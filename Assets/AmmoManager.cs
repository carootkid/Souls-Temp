using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AmmoManager : MonoBehaviour
{
    public int ammoAmount = 100;
    public int maxAmmo = 100;

    public TextMeshProUGUI ammoText;

    private void Update()
    {
        ammoText.text = "" + ammoAmount;

        if(ammoAmount > maxAmmo)
        {
            ammoAmount = maxAmmo;
        }
    }
}
