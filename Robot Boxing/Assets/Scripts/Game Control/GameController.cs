using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
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

    [SerializeField] private GameObject playerGauge;
    [SerializeField] private GameObject opponentGauge;
    [SerializeField] private GameObject timerGauge;
    [SerializeField] private GameObject roundGauge;

    [Header("Fight State")]
    public float difficulty = 1;
    public int round = 0;
    public int match = 0;
    [SerializeField] private int maxMatches = 6;
    [SerializeField] private int maxRounds = 6;

    public AudioClip RepairBackground;
    public AudioClip FightPrepBackground;
    public AudioClip FightBackground;

    private AudioClip currentClip;
    private void SetBackgroundMusic(AudioClip clip, float volumne, float delayIfStart = 0)
    {
        var audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = volumne;
        if (clip == currentClip)
            return;
        audioSrc.Stop();
        audioSrc.clip = clip;
        audioSrc.loop = true;
        audioSrc.PlayDelayed(delayIfStart);

    }

    private void Start()
    {
        playerGauge = GameObject.FindGameObjectWithTag("Player Gauge");
        opponentGauge = GameObject.FindGameObjectWithTag("Enemy Gauge");
        timerGauge = GameObject.FindGameObjectWithTag("Timer Gauge");
        roundGauge = GameObject.FindGameObjectWithTag("Match Gauge");
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
        roundGauge.transform.GetChild(0).GetComponent<Text>().text =  "Match " + (int)(match+1) + " | Round " + (int)(round+1);
        timerDisplay.enabled = true;
        string curScene = SceneManager.GetActiveScene().name;
        if(curScene != "Fight Scene" && curScene != "MainMenu"){
            timer -= Time.deltaTime;
            timerDisplay.text = "" + timer;
            if(timer <= 0)//load the fight if in any repair scene
                LoadFightScene();
        }
        else
            timerDisplay.text = "0:00";

        HideUI();

    }

    public void CheckConditions(){
        IsPlayerWonFight();
        IsPlayerWonGame();
        IsPlayerLostGame();
    }

    public void LoadFightScene(){
        SetBackgroundMusic(FightPrepBackground, 1);
        timer = 0;
        SceneManager.LoadScene("Fight Scene", LoadSceneMode.Single);
    }
    public void LoadRepairMenu(){
        SetBackgroundMusic(RepairBackground, 1);
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

    private void HideUI(){
        if(SceneManager.GetActiveScene().name == "Fight Scene"){
            //Show Enemy Meter
            opponentGauge.SetActive(true);
            roundGauge.SetActive(true);
            //Move Player Meter
            playerGauge.GetComponent<RectTransform>().anchorMin = new Vector2(0,0.5f);
            playerGauge.GetComponent<RectTransform>().anchorMax = new Vector2(0,0.5f);
            playerGauge.SetActive(true);
            playerGauge.GetComponent<RectTransform>().anchoredPosition3D= new Vector3(57,0,0);
            timerGauge.SetActive(false);
        }
        else if(SceneManager.GetActiveScene().name == "Repair Menu"){
            //Hide Enemy Meter
            roundGauge.SetActive(false);
            opponentGauge.SetActive(false);
            //Move Player Meter
            playerGauge.SetActive(true);
            playerGauge.GetComponent<RectTransform>().anchorMin = new Vector2(1,0.5f);
            playerGauge.GetComponent<RectTransform>().anchorMax = new Vector2(1,0.5f);
            playerGauge.GetComponent<RectTransform>().anchoredPosition3D= new Vector3(0,0,0);
            // move timer
            timerGauge.SetActive(true);
            timerGauge.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f,1);
            timerGauge.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f,1);
            timerGauge.GetComponent<RectTransform>().anchoredPosition3D= new Vector3(0,-45,0);
        }
        else if(SceneManager.GetActiveScene().name == "HeadRepair"){
            //Hide Enemy Meter
            opponentGauge.SetActive(false);
            //Hide Player Meter
            playerGauge.SetActive(false);
            //move timer;
            timerGauge.GetComponent<RectTransform>().anchorMin = new Vector2(0.75f,0.25f);
            timerGauge.GetComponent<RectTransform>().anchorMax = new Vector2(0.75f,0.25f);
            timerGauge.GetComponent<RectTransform>().anchoredPosition3D= new Vector3(0,0,0);
        }
        else{
            roundGauge.SetActive(false);
            //Hide Enemy Meter
            opponentGauge.SetActive(false);
            //Hide Player Meter
            playerGauge.SetActive(false);
            //Hide timer
            timerGauge.SetActive(false);
        }
        

    }

    

}
