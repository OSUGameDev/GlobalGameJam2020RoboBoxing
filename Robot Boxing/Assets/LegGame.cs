using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegGame : MonoBehaviour
{
    List<int> Sequence = new List<int>();
    List<int> Player_Sequence = new List<int>();
    GameObject background;
    float timer = 0;
    float time = .5f;
    bool blue = false;
    bool green = false;
    bool red = false;
    bool yellow = false;
    bool wait = true;
    int counter = 0;
    bool done = false;
    bool broken = false;

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
    }

    // Update is called once per frame
    void Update()
    {

        if (done == true)
        {
            for (int i = 0; i < Sequence.Count; i++)
            {
                if (Sequence[i] != Player_Sequence[i])
                {
                    Debug.Log("Broken");
                    broken = true;
                }
            }
            if (broken == true)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LegRepair", UnityEngine.SceneManagement.LoadSceneMode.Single);
            } else
            {
                GameController.Instance.player.ModifyLeg(25);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Repair Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }

        // Debug.Log(time.ToString() + " " + timer.ToString());
        timer += Time.deltaTime;
        if (wait == true)
        {
            if (time < timer)
            {
                wait = false;
                timer = 0;
                lightOn(Sequence[counter]);
                counter++;
            }
        } else if (blue == true)
        {
            if (time < timer)
            {
                lightOff(0);
            }
        } else if (green == true)
        {
            if (time < timer)
            {
                lightOff(1);
            }
        }
        else if (red == true)
        {
            if (time < timer)
            {
                lightOff(2);
            }
        }
        else if (yellow == true)
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
            blue = true;
        } else if (button == 1)
        {
            background.transform.Find("GreenLight").GetComponent<SpriteRenderer>().sprite = greenlight_0;
            green = true;
        }
        else if (button == 2)
        {
            background.transform.Find("RedLight").GetComponent<SpriteRenderer>().sprite = redlight_0;
            red = true;
        }
        else if (button == 3)
        {
            background.transform.Find("YellowLight").GetComponent<SpriteRenderer>().sprite = yellowlight_0;
            yellow = true;
        } else
        {
            Debug.Log("Error: Out of scope");
        }
    }

    void lightOff(int button)
    {
        timer = 0;
        wait = true;
        if (button == 0)
        {
            background.transform.Find("BlueLight").GetComponent<SpriteRenderer>().sprite = bluelight_1;
            blue = false;
        }
        else if (button == 1)
        {
            background.transform.Find("GreenLight").GetComponent<SpriteRenderer>().sprite = greenlight_1;
            green = false;
        }
        else if (button == 2)
        {
            background.transform.Find("RedLight").GetComponent<SpriteRenderer>().sprite = redlight_1;
            red = false;
        }
        else if (button == 3)
        {
            background.transform.Find("YellowLight").GetComponent<SpriteRenderer>().sprite = yellowlight_1;
            yellow = false;
        }
        else
        {
            Debug.Log("Error: Out of scope");
        }
    }

    public void buttonPressedBlue()
    {
        Player_Sequence.Add(0);
        if (Sequence.Count == Player_Sequence.Count) {
            done = true;
        }
    }
    public void buttonPressedGreen()
    {
        Player_Sequence.Add(1);
        if (Sequence.Count == Player_Sequence.Count)
        {
            done = true;
        }
    }
    public void buttonPressedRed()
    {
        Player_Sequence.Add(2);
        if (Sequence.Count == Player_Sequence.Count)
        {
            done = true;
        }
    }
    public void buttonPressedYellow()
    {
        Player_Sequence.Add(3);
        if (Sequence.Count == Player_Sequence.Count)
        {
            done = true;
        }
    }
}
