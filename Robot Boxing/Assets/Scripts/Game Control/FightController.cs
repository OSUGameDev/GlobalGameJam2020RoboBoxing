using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class FightController : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    private GameController GameController;
    private float timer;
    [SerializeField] private float attackTime = 0.3f;
    private bool attacking = false;
    private bool startRound = false;

    private int numHits_opponent;
    private int numHits_player;
    private bool playerAttacked = false;
    private bool opponentAttacked = false;

    [SerializeField] private Fighter opponent;
    [SerializeField] private Animator player_anim;
    [SerializeField] private Animator opponent_anim;


    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        startButton.onClick.AddListener(StartButton);
        opponent = GameController.curOpponent;
        numHits_player = Random.Range(2,5);
        numHits_opponent = Random.Range(2,5);
    }

    void Update(){
        //set up a basic timer that activates when attacking becomes true;
        if(attacking && timer < attackTime){
            timer += Time.deltaTime;
        }
        else{
            if(playerAttacked)
                playerAttacked = false;
            if(opponentAttacked)
                opponentAttacked = false;
            attacking = false;
            timer = 0;
        }
        
        if(startRound)
            CalculateHits();
    }

    void CalculateHits(){

        if(numHits_player > 0 && !playerAttacked && !attacking){
            int attack = 0;
            switch (GameController.player.focus){
                case 0: //attacks any part randomly
                    attack = Random.Range(0,5);
                    break;
                case 1: //attacks any part that the opponent uses to attack
                    attack = Random.Range(0,1);
                    break;
                case 2: //attacks any part that the opponent uses for accuracy
                    attack = Random.Range(2,3);
                    break;
                case 3: //attacks any part that the opponent uses to dodge
                    attack = Random.Range(4,5);
                    break;
            }
            switch (attack){
                case 0:
                    AttackArms(GameController.player,opponent);
                    break;
                case 1:
                    AttackLegs(GameController.player,opponent);
                    break;
                case 2:
                    AttackCables(GameController.player,opponent);
                    break;
                case 3:
                    AttackCoolant(GameController.player,opponent);
                    break;
                case 4:
                    AttackEyes(GameController.player,opponent);
                    break;
                case 5:
                    AttackCircuits(GameController.player,opponent);
                    break;
            }
            numHits_player--;
            playerAttacked = true;
            attacking = true;
            player_anim.Play("player_punch");
        }

        if(numHits_opponent > 0 && !opponentAttacked && !attacking){
            //choose part to attack based on focus (set either randomly or by player)
            int attack = 0;
            switch (opponent.focus){
                case 0: //attacks any part randomly
                    attack = Random.Range(0,6);
                    break;
                case 1: //attacks any part that the opponent uses to attack
                    attack = Random.Range(0,2);
                    break;
                case 2: //attacks any part that the opponent uses for accuracy
                    attack = Random.Range(2,4);
                    break;
                case 3: //attacks any part that the opponent uses to dodge
                    attack = Random.Range(4,6);
                    break;
            }
            
            switch (attack){
                case 0:
                    AttackArms(opponent,GameController.player);
                    break;
                case 1:
                    AttackLegs(opponent,GameController.player);
                    break;
                case 2:
                    AttackCables(opponent,GameController.player);
                    break;
                case 3:
                    AttackCircuits(opponent,GameController.player);
                    break;
                case 4:
                    AttackEyes(opponent,GameController.player);
                    break;
                case 5:
                    AttackCoolant(opponent,GameController.player);
                    break;
            }
                numHits_opponent--;
                opponentAttacked = true;
                attacking = true;
        }
        
        if(numHits_opponent == 0 && numHits_player == 0 && timer <= 0){
            GameController.curOpponent = opponent;
            GameController.round++;
            GameController.LoadRepairMenu();
        }
    }

    
    void StartButton(){
        startRound = true;
        startButton.GetComponent<Image>().enabled = false;
        startButton.enabled = false;
        
    }

    void AttackArms(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyArm(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s arms and does " + (int)(-damage) + " damage!");
    }

    void AttackLegs(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyLeg(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s legs and does " + (int)(-damage) + " damage!");
    }

    void AttackCables(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCableBox(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s cables and does " + (int)(-damage) + " damage!");
    }

    void AttackCircuits(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCircuitBoard(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s circuits and does " + (int)(-damage) + " damage!");
    }

    void AttackCoolant(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCoolant(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s coolant cell and does " + (int)(-damage) + " damage!");
    }

    void AttackEyes(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender); 
        defender.ModifyEyes(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s eyes and does " + (int)(-damage) + " damage!");
    }

    private float AttackCalclate(Fighter attacker,Fighter defender){
        float accuracy = (0.01f*attacker.cableBox)+(0.01f*attacker.circuitBoard);
        accuracy = Mathf.Clamp(accuracy,0.01f,1);
        float damage = 5 * -(attacker.arm/defender.eyes + attacker.leg/defender.coolant) * accuracy;
        return damage;
    }

}