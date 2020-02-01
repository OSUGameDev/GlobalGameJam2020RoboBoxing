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
    [SerializeField] private Button repairButton;
    private GameController GameController;

    void Start()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        repairButton.onClick.AddListener(RepairButton);
    }
    //Testing Purposes Only
    void RepairButton(){
        GameController.LoadRepairMenu();
    }

}