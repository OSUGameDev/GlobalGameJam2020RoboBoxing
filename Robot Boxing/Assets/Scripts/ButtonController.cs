using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    [Header("Stat Buttons")]
    [SerializeField] private Button defenceButton;
    [SerializeField] private Button sightButton;
    [SerializeField] private Button grappleButton;
    [SerializeField] private Button strengthButton;
    [SerializeField] private Button speedButton;
    [SerializeField] private Button pierceButton;
    //Any buttoons that will open a menu
    [Header("Menu Buttons")]
    [SerializeField] private Button fightButton;
    private GameController GameController;

    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //adding event listeners cause we javascript now boys
        defenceButton.onClick.AddListener(DefenceButton);
        grappleButton.onClick.AddListener(GrappleButton);
        sightButton.onClick.AddListener(SightButton);
        strengthButton.onClick.AddListener(StrengthButton);
        speedButton.onClick.AddListener(SpeedButton);
        pierceButton.onClick.AddListener(PierceButton);
        fightButton.onClick.AddListener(FightButton);
    }

    void Update(){
        GameController.timer = GameController.timer -= Time.deltaTime;
        //if timer reaches 0... start fight
        if(GameController.timer <= 0)
            FightButton();
    }

    /*
        Each button has it's own function because it's easier and we might have certain buttons do more than one thing and other such things I guess, probably
        could have had a listener function that took a parameter and put it in a case statement, but I want to keep it like this for now.
    */
    void DefenceButton()
    {

        GameController.ModifyDefence(1);
    }
    void GrappleButton()
    {

        GameController.ModifyGrapple(1);
    }
    void SightButton()
    {

        GameController.ModifySight(1);
    }
    void StrengthButton()
    {

        GameController.ModifyStrength(1);
    }
    void SpeedButton()
    {

        GameController.ModifySpeed(1);
    }
    void PierceButton()
    {
        
        GameController.ModifyPierce(1);
    }
    void FightButton(){
        //Load the fight
        GameController.LoadFightScene();
    }

}