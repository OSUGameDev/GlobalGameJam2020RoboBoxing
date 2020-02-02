using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_panel : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform rT;
    private GameController GameController;
    private Vector2 oDims;

    void Start(){
        rT = GetComponent<RectTransform>();
        oDims = rT.sizeDelta;
        GameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.tag){
            case("arm meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.arm*0.01f*oDims.x,oDims.y);
                break;
            case("eye meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.eyes*0.01f*oDims.x,oDims.y);
                break;
            case("cable meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.cableBox*0.01f*oDims.x,oDims.y);
                break;
            case("circuit meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.circuitBoard*0.01f*oDims.x,oDims.y);
                break;
            case("leg meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.leg*0.01f*oDims.x,oDims.y);
                break;
            case("coolant meter"):
                rT.sizeDelta = new Vector2(GameController.curOpponent.coolant*0.01f*oDims.x,oDims.y);
                break;
        }
    }
}
