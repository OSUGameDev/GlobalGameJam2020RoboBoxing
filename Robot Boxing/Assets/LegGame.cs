using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegGame : MonoBehaviour
{
    List<int> Sequence = new List<int>();
    GameObject background;
    float timer = 0;
    float time = 0;
    bool blue = false;
    bool green = false;
    bool red = false;
    bool yellow = false;
    int counter = 0;

    [SerializeField]
    Sprite bluelight_0;
    [SerializeField]
    Sprite bluelight_1;
    [SerializeField]
    Sprite greenlight_0;
    [SerializeField]
    Sprite greenlight_1;
    [SerializeField]
    Sprite redlight_0;
    [SerializeField]
    Sprite redlight_1;
    [SerializeField]
    Sprite yellowlight_0;
    [SerializeField]
    Sprite yellowlight_1;
    // Start is called before the first frame update
    void Start()
    {
        background = GameObject.Find("BackgroundCanvas");
        //float match2 = (float)GameObject.Find("GameController").GetComponent<GameController>().match + 1.0f;
        for (int i = 0; i < 5; i++)
        {
            Sequence.Add(Random.Range(0, 4));
        }
        lightOn(Sequence[counter]);
        counter++;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(counter);
        timer += Time.deltaTime;
        if (blue == true && time > 0)
        {
            if (time < timer)
            {
                lightOff(0);
            }
        } else if (green == true && time > 0)
        {
            if (time < timer)
            {
                lightOff(1);
            }
        }
        else if (red == true && time > 0)
        {
            if (time < timer)
            {
                lightOff(2);
            }
        }
        else if (yellow == true && time > 0)
        {
            if (time < timer)
            {
                lightOff(3);
            }
        } else
        {
            timer = 0;
        }
    }

    void lightOn(int button)
    {
        timer = 0;
        if (button == 0)
        {
            background.transform.Find("BlueLight").GetComponent<SpriteRenderer>().sprite = bluelight_0;
            time = 1;
            blue = true;
        } else if (button == 1)
        {
            background.transform.Find("GreenLight").GetComponent<SpriteRenderer>().sprite = greenlight_0;
            time = 1;
            green = true;
        }
        else if (button == 2)
        {
            background.transform.Find("RedLight").GetComponent<SpriteRenderer>().sprite = redlight_0;
            time = 1;
            red = true;
        }
        else if (button == 3)
        {
            background.transform.Find("YellowLight").GetComponent<SpriteRenderer>().sprite = yellowlight_0;
            time = 1;
            yellow = true;
        } else
        {
            Debug.Log("Error: Out of scope");
        }
    }

    void lightOff(int button)
    {
        if (button == 0)
        {
            background.transform.Find("BlueLight").GetComponent<SpriteRenderer>().sprite = bluelight_1;
            blue = false;
            if (Sequence.Count > counter)
            {
                time = 1;
                lightOn(Sequence[counter]);
                counter++;
            }
        }
        else if (button == 1)
        {
            background.transform.Find("GreenLight").GetComponent<SpriteRenderer>().sprite = greenlight_1;
            green = false;
            if (Sequence.Count > counter)
            {
                time = 1;
                lightOn(Sequence[counter]);
                counter++;
            }
        }
        else if (button == 2)
        {
            background.transform.Find("RedLight").GetComponent<SpriteRenderer>().sprite = redlight_1;
            red = false;
            if (Sequence.Count > counter)
            {
                time = 1;
                lightOn(Sequence[counter]);
                counter++;
            }
        }
        else if (button == 3)
        {
            background.transform.Find("YellowLight").GetComponent<SpriteRenderer>().sprite = yellowlight_1;
            yellow = false;
            if (Sequence.Count > counter)
            {
                time = 1;
                lightOn(Sequence[counter]);
                counter++;
            }
        }
        else
        {
            Debug.Log("Error: Out of scope");
        }
    }

    public void buttonPressedBlue()
    {
        
    }
    public void buttonPressedGreen()
    {
        
    }
    public void buttonPressedRed()
    {
        
    }
    public void buttonPressedYellow()
    {
        
    }
}
