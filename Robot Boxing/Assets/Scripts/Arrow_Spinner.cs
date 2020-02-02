using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Spinner : MonoBehaviour
{
    // Start is called before the first frame update

    float Target_Pressure;
    float Click;
    float Current_Increment;
    GameObject arrow;
    void Start()
    {
        arrow = GameObject.Find("Arrow"); //build error happens if you do it during serialization
        float match2 = (float)GameObject.Find("GameController").GetComponent<GameController>().match + 1.0f;
        GameObject.Find("GameController").GetComponentInChildren<Canvas>().enabled = false;
        Debug.Log(match2);
        Target_Pressure = 1000 * match2 + Random.Range(0, 11) * match2;
        Click = Random.Range(0, 10 * match2) + 100;
        Current_Increment = 0;
        arrow = GameObject.Find("Arrow");
    }

    // Update is called once per frame
    public void Increment()
    {
        time += Click;
    }

    void Update()
    {
        if (time > 0)
        {
            Current_Increment += 1;
            time -= 1;
            if (time > 25)
            {
                Current_Increment += 1;
                time -= 1;
                if (time > 100)
                {
                    Current_Increment += 1;
                    time -= 1;
                    if (time > 150)
                    {
                        Current_Increment += 1;
                        time -= 1;
                        if (time > 200)
                        {
                            Current_Increment += 1;
                            time -= 1;
                        }
                    }
                }
            }
            float test = (-360 * (Current_Increment / Target_Pressure));
            Quaternion zero = Quaternion.Euler(0, 0, test);
            arrow.transform.rotation = zero;
        }

        if (time <= 0)
        {
            float test = (360 * (Current_Increment / Target_Pressure));
            float test2 = test % 360;
            if (test2 >= 295 && test2 <= 332.5)
            {
                GameObject.Find("GameController").GetComponentInChildren<Canvas>().enabled = true;
                GameController.Instance.player.ModifyArm(25);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Repair Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

    }
}
