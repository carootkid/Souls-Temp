using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpScript : MonoBehaviour
{
    public int playerLevel = 0;
    public int playerEXP = 0;
    private int neededEXP = 100;
    public int EXPincrease = 10;

    void Start()
    {
        
    }

    void Update()
    {
        if(playerEXP == neededEXP + EXPincrease){
            Debug.Log("leveled UP");
            playerLevel++;
            EXPincrease = EXPincrease * playerLevel;
        }
    }
}
