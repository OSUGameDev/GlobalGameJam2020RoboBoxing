using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }

    [Header("Player & Opponent")]
    public Fighter player;
    public Fighter curOpponent = null;



    [Header("Repair Timer")]
    public float timer = 0; //for repair minigames
    [SerializeField] private float repairTime; //time allowed for player to repair the robot
    [SerializeField] private Text timerDisplay;

    [Header("Fight State")]
    public float difficulty = 1;
    public int round = 0;
    public int match = 0;
    [SerializeField] private int maxMatches = 6;
    [SerializeField] private int maxRounds = 6;


    private void Start()
    {
        player = GeneratePlayer();
        curOpponent = GenerateFighter();
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update(){
        timerDisplay.enabled = true;
        if(SceneManager.GetActiveScene().name == "Repair Menu")
            timerDisplay.text = "" + timer;
        else
            timerDisplay.enabled = false;
        IsPlayerWonFight();
        IsPlayerWonGame();
        IsPlayerLostGame();

    }

    public void LoadFightScene(){
        timer = 0;
        SceneManager.LoadScene("Fight Scene", LoadSceneMode.Single);
    }
    public void LoadRepairMenu(){
        timer = repairTime; //set the timer
        SceneManager.LoadScene("Repair Menu", LoadSceneMode.Single);
        
    }

    public void IsPlayerWonFight(){
        if(round > maxRounds && curOpponent.IsFighterLostByScore(player) || curOpponent.IsFighterLostByDamage()){
            match++;
            curOpponent = GenerateFighter();
            round = 0;
        }
    }

    public void IsPlayerWonGame(){
        //if reached past the max number of matches
        if(match > maxMatches)
            EndGame(true);
    }

    public void IsPlayerLostGame(){
        if(player.IsFighterLostByDamage() || round > maxRounds && player.IsFighterLostByScore(curOpponent))
            EndGame(false);
    }

    //maybe will do something special if the player has won, but for now will do nothing
    public void EndGame(bool hasWon){
        //reset everything so the player an go again.
        player = GeneratePlayer();
        curOpponent = GenerateFighter();
        match = 0;
        round = 0;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    //generate a fighter with varying stats increasing by difficulty
    public Fighter GeneratePlayer(){
        Fighter f = new Fighter();
        f.name = "player's robot";
        return f;
    }

    //Generates a new fighter
    public Fighter GenerateFighter(){
        int handicap = 0;
        if(match > 0)
            handicap = 1;
        //difficulty coefficient
        float c = match * difficulty + 1;
        Fighter f = new Fighter();
        f.name = "opponent's robot";
        f.arm =             75 + handicap * 25;
        f.leg =             75 + handicap * 25;
        f.cableBox =        75 + handicap * 25;
        f.circuitBoard =    75 + handicap * 25; 
        f.eyes =            75 + handicap * 25; 
        f.coolant =         75 + handicap * 25;
        f.focus = Random.Range(1,3);
        return f;
    }

    

}
