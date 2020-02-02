using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fighter
{
    [Header("Personal Details")]
    public string name;
    public string intro;
    public string weaknesses;
    public string ownerName;
 

    [Header("Stats")]
    public float eyes = 100;
    public float cableBox = 100;
    public float arm = 100;
    public float circuitBoard = 100;
    public float leg = 100;
    public float coolant = 100;
    public float focus = 0; //0 = all | 1 = disable attack | 2 = disable movement | 3 = disable senses


    public void ModifyEyes(float n){
        eyes += (int)(n);
        eyes = Mathf.Clamp(eyes,0,100);       // Debug.Log("Eyes Value is now " + eyes);
    }
    public void ModifyArm(float n){
        arm += (int)(n);
        arm = Mathf.Clamp(arm,0,100);  
        //Debug.Log("Arm Value is now " + arm);
    }
    public void ModifyCableBox(float n){
        cableBox += (int)(n);
        cableBox = Mathf.Clamp(cableBox,0,100);  
      //  Debug.Log("Cablebox Value is now " + cableBox);
    }
    public void ModifyCircuitBoard(float n){
        circuitBoard += (int)(n);
        circuitBoard = Mathf.Clamp(circuitBoard,0,100);  
        //Debug.Log("CircuitBoard Value is now " + circuitBoard);
    }
    public void ModifyLeg(float n){
        leg += (int)(n);
        leg = Mathf.Clamp(leg,0,100);  
       // Debug.Log("Leg Value is now " + leg);
    }
    public void ModifyCoolant(float n){
        coolant += (int)(n);
        coolant = Mathf.Clamp(coolant,0,100);  
      //  Debug.Log("Coolant Value is now " + coolant);
    }
    public bool IsFighterLostByDamage(){
        if(arm <= 1 && leg <= 1 || cableBox <= 1 && circuitBoard <= 1 || coolant <= 1 && eyes <= 1){
            Debug.Log(name + " is unable to fight!");
            return true;
        }
        return false;
    }
    public bool IsFighterLostByScore(Fighter opponent){
        //add up the total remaining skill numbers and compare them
        float myScore = (arm+leg+cableBox+circuitBoard+eyes+coolant);
        float opponentScore = (opponent.arm+opponent.leg+opponent.cableBox+opponent.circuitBoard+opponent.eyes+opponent.coolant);
        if(myScore <= opponentScore){
            Debug.Log(name + " has been defeated by " + opponent.name + " by score!");
            return true;
        }
        return false;

    }
}
