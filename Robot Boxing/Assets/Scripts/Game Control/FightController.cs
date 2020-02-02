using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AudioInformation
{
    public AudioClip clip;
    public bool loop;
    public float volumne;
    public float startTime;
    public float endTime;
}

public class FightController : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    [Header("Menu Buttons")]
    [SerializeField] private Button startButton;
    private GameController GameController;
    private float timer;
    private float delayTime;
    [SerializeField] private float attackTime = 0.3f;
    private bool attacking = false;
    private bool startRound = false;

    private int numHits_opponent;
    private int numHits_player;
    private int turn = 0;

    [SerializeField] private Fighter opponent;
    [SerializeField] private Animator player_anim;
    [SerializeField] private Animator opponent_anim;

    public AudioInformation waitingSoundInfo;
    public AudioInformation fightingSoundInfo;



    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        startButton.onClick.AddListener(StartButton);
        opponent = GameController.curOpponent;
        numHits_player = Random.Range(2,5);
        numHits_opponent = Random.Range(2,5);

        if(GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().clip = waitingSoundInfo.clip;
            GetComponent<AudioSource>().loop = waitingSoundInfo.loop;
            GetComponent<AudioSource>().volume = waitingSoundInfo.volumne;
            GetComponent<AudioSource>().Play();
        }
    }

    void Update(){
        //set up a basic timer that activates when attacking becomes true;
        UpdateDelayTimer();
        if(delayTime > 0)
            Debug.Log("Delaying = " + delayTime);
        else{                
                if(startRound){
                    CalculateHits();
                }
            }
        }

    void Delay(float delay){
        delayTime = delay;
    }
    void UpdateDelayTimer(){
        delayTime -= Time.deltaTime;
    }

    void CalculateHits(){

        if(numHits_player > 0 && turn == 0){
            int attack = 0;
            float damage = 0;
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
                    damage =AttackArms(GameController.player,opponent);
                    break;
                case 1:
                    damage =AttackLegs(GameController.player,opponent);
                    break;
                case 2:
                    damage =AttackCables(GameController.player,opponent);
                    break;
                case 3:
                    damage =AttackCoolant(GameController.player,opponent);
                    break;
                case 4:
                    damage =AttackEyes(GameController.player,opponent);
                    break;
                case 5:
                    damage =AttackCircuits(GameController.player,opponent);
                    break;
            }
            //Hit sounds go here
            numHits_player--;
            Delay(attackTime);
            turn = 1;
            if(attack >= 3)
                player_anim.Play("player_punch");
            else
                player_anim.Play("player_jab");
        }

        if(numHits_opponent > 0 && turn == 1){
            //choose part to attack based on focus (set either randomly or by player)
            int attack = 0;
            float damage;
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
                    damage = AttackArms(opponent,GameController.player);
                    break;
                case 1:
                    damage =AttackLegs(opponent,GameController.player);
                    break;
                case 2:
                    damage =AttackCables(opponent,GameController.player);
                    break;
                case 3:
                    damage =AttackCircuits(opponent,GameController.player);
                    break;
                case 4:
                    damage =AttackEyes(opponent,GameController.player);
                    break;
                case 5:
                    damage =AttackCoolant(opponent,GameController.player);
                    break;
            }
                opponent_anim.Play("enemy_jab");
                numHits_opponent--;
                Delay(attackTime);
                turn = 0;
        }

        if(GameController.IsPlayerWonFight()){
            GameController.ToggleWin(true);
            Delay(5);
        }
        
        if(numHits_opponent == 0 && numHits_player == 0 && delayTime <= 0){
            if(GameController.winSign.activeInHierarchy)
                GameController.ToggleWin(true);
            if(GameController.loseSign.activeInHierarchy)
                GameController.ToggleWin(false);
            GameController.curOpponent = opponent;
            GameController.round++;
            GameController.CheckConditions(); //If this passes then we will go to main menu instead
            GameController.LoadRepairMenu();
        }
    }

    
    void StartButton(){
        startRound = true;
        startButton.GetComponent<Image>().enabled = false;
        startButton.enabled = false;
        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = fightingSoundInfo.clip.MakeSubclip(fightingSoundInfo.startTime, fightingSoundInfo.endTime);
            GetComponent<AudioSource>().loop = fightingSoundInfo.loop;
            GetComponent<AudioSource>().volume = fightingSoundInfo.volumne;
            GetComponent<AudioSource>().Play();
        }
     }

    float AttackArms(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyArm(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s arms and does " + (int)(-damage) + " damage!");
        return damage;
    }

    float AttackLegs(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyLeg(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s legs and does " + (int)(-damage) + " damage!");
        return damage;
    }

    float AttackCables(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCableBox(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s cables and does " + (int)(-damage) + " damage!");
        return damage;
    }

    float AttackCircuits(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCircuitBoard(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s circuits and does " + (int)(-damage) + " damage!");
        return damage;
    }

    float AttackCoolant(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender);
        defender.ModifyCoolant(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s coolant cell and does " + (int)(-damage) + " damage!");
        return damage;
    }

    float AttackEyes(Fighter attacker, Fighter defender){
        float damage = AttackCalclate(attacker,defender); 
        defender.ModifyEyes(damage);
        Debug.Log(attacker.name + " attacks " + defender.name + "'s eyes and does " + (int)(-damage) + " damage!");
        return damage;
    }

    private float AttackCalclate(Fighter attacker,Fighter defender){
        float accuracy = (0.01f*attacker.cableBox)+(0.01f*attacker.circuitBoard);
        accuracy = Mathf.Clamp(accuracy,0.01f,1);
        float damage = 5 * -(attacker.arm/defender.eyes + attacker.leg/defender.coolant) * accuracy;
        return damage;
    }

}