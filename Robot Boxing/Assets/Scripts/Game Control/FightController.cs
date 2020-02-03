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
    public float delayTime;
    [SerializeField] private float attackTime = 0.3f;
    private bool attacking = false;
    private bool startRound = false;

    private int numHits_opponent;
    private int numHits_player;
    private int turn = 0;
    private bool lostGame = false;
    private bool opponentDefeated = false;

    [SerializeField] private Fighter opponent;
    [SerializeField] private Animator player_anim;
    [SerializeField] private Animator opponent_anim;

    public AudioClip BellSound;
    public float bellVolume;
    public float hitVolume = 0.5f;
    public float fightSoundDelay = 0;

    [SerializeField] public AudioClip[] hitNoises;
    [SerializeField] public AudioClip[] voiceNoises;
    


    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        startButton.onClick.AddListener(StartButton);
        opponent = GameController.curOpponent;
        numHits_player = Random.Range(2,5);
        numHits_opponent = Random.Range(2,5);
        GameController.Instance.SetBackgroundMusic(GameController.Instance.FightPrepBackground,GameController.backgroundVolume,0);
    }

    void Update(){
        //set up a basic timer that activates when attacking becomes true;
        UpdateDelayTimer();
        if(delayTime <= 0){         
                if(startRound){
                    CalculateHits();
                }
                
        }
        CheckConditions();
    }

    void PlayHit(){
        int n = hitNoises.Length;
        int r = Random.Range(0,n-1);
        AudioSource.PlayClipAtPoint(hitNoises[r], transform.position, bellVolume);
        //play voices
        n = voiceNoises.Length;
        r = Random.Range(0,n-1);
        AudioSource.PlayClipAtPoint(voiceNoises[r], transform.position, bellVolume);
    }

    void Delay(float delay){
        delayTime = delay;
    }
    void UpdateDelayTimer(){
        delayTime -= Time.deltaTime;
    }

    void CalculateHits(){
        if(numHits_player <= 0)
            turn = 1;
        if(numHits_opponent <= 0)
            turn = 0;


        if(numHits_player > 0 && delayTime <= 0){
            int attack = 0;
            float damage = 0;
            switch (0){
                case 0: //attacks any part randomly
                    attack = Random.Range(0,6);
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
            if(attack > 1)
                player_anim.Play("player_punch");
            else if(attack > 3)
                player_anim.Play("player_jab");
            else
                player_anim.Play("player_swipe");
            if(hitNoises.Length > 0)
                PlayHit();
        }

        if(numHits_opponent > 0 && delayTime <= 0){
            //choose part to attack based on focus (set either randomly or by player)
            int attack = 0;
            float damage;
            switch (0){
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
                if(attack >= 3)
                    opponent_anim.Play("enemy_jab");
                else
                    opponent_anim.Play("badguy_swipe");
                if(hitNoises.Length > 0)
                    PlayHit();
                numHits_opponent--;
                Delay(attackTime);
                turn = 0;
        }

    }
    private void CheckConditions(){

        if(!opponentDefeated && numHits_opponent == 0 && numHits_player == 0 && opponent.IsFighterLostByDamage() || opponent.IsFighterLostByScore(GameController.player) && GameController.round >= 6){
            opponent_anim.SetTrigger("badGuy_knockout");
            opponentDefeated = true;
            Delay(3);
        }
        if(!lostGame){
            if(numHits_opponent == 0 && numHits_player == 0 && GameController.IsPlayerWonFight()){
                GameController.ToggleWin(true);
                Delay(3);
            }
            if(numHits_opponent == 0 && numHits_player == 0 && GameController.IsPlayerLostGame()){
                lostGame = true;
                player_anim.SetTrigger("player_knockout");
                GameController.ToggleWin(false);
                Delay(3);
            }
            
            if(numHits_opponent == 0 && numHits_player == 0 && delayTime <= 0){
                if(GameController.winSign.activeInHierarchy)
                    GameController.ToggleWin(true);
                if(GameController.loseSign.activeInHierarchy)
                    GameController.ToggleWin(false);
                GameController.round++;
                opponentDefeated = false;
                GameController.curOpponent = opponent;
                GameController.LoadRepairMenu();
            }
        }
        else if(delayTime <= 0)
            GameController.IsPlayerLostGame();

    }

    
    void StartButton(){
        startRound = true;
        startButton.gameObject.SetActive(false);
        startButton.enabled = false;
        var audioSrc = GameController.Instance.GetComponent<AudioSource>();
        audioSrc.volume = .2f;
        AudioSource.PlayClipAtPoint(BellSound, transform.position, bellVolume);
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