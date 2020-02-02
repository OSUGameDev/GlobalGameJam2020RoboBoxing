using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Spinner : MonoBehaviour
{
    // Start is called before the first frame update

    float Target_Pressure;
    float Click;
    float Current_Increment;
    void Start()
    {
        float match2 = (float)GameObject.Find("GameController").GetComponent<GameController>().match + 1.0f;
        Debug.Log(match2);
        Target_Pressure = 1000 * match2 + Random.Range(0, 11) * match2;
        Click = Random.Range(0, 10) + 10;
        Current_Increment = 0;
    }

    // Update is called once per frame
    public void Increment()
    {
        Current_Increment += Click;
        Debug.Log(Current_Increment);
        GameObject arrow = GameObject.Find("Arrow");
        float test = (-360 * (Current_Increment / Target_Pressure));
        Debug.Log(test);
        Quaternion zero = Quaternion.Euler(0, 0, test);
        arrow.transform.rotation = zero;
    }
}
