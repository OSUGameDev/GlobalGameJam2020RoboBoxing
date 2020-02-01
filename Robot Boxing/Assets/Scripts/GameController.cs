using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;

    public static GameController Instance { get { return _instance; } }

    [Header("Stats")]
    public float defence = 0;
    public float sight = 0;
    public float grapple = 0;
    public float strength = 0;
    public float speed = 0;
    public float pierce = 0;

    [Header("Repair Timer")]
    public float timer = 0; //for repair minigames
    [SerializeField] private float repairTime; //time allowed for player to repair the robot
    [SerializeField] private Text timerDisplay;


    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update(){
        timerDisplay.text = "" + timer;
    }

    public void ModifyDefence(float n){
        defence += n;
    }
    public void ModifyGrapple(float n){
        grapple += n;
    }
    public void ModifySight(float n){
        sight += n;
    }
    public void ModifyStrength(float n){
        strength += n;
    }
    public void ModifySpeed(float n){
        speed += n;
    }
    public void ModifyPierce(float n){
        pierce += n;
    }

    public void LoadFightScene(){
        timer = 0;
        SceneManager.LoadScene("Fight Scene", LoadSceneMode.Single);
    }
    public void LoadRepairMenu(){
        timer = repairTime; //set the timer
        SceneManager.LoadScene("Repair Menu", LoadSceneMode.Single);
        
    }

}
