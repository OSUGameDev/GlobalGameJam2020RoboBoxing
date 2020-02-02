using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMiniGameController : MonoBehaviour
{

    public float Radius;
    public int NumbScrews;
    public GameObject ScrewPrefab;
    private int numScrewsDone;
    public GameObject CrackedEye;
    public GameObject NewEye;
    bool isScrewingIn;
    // Start is called before the first frame update
    List<EyeMiniGameScrewController> screws;
    void Start()
    {
        isScrewingIn = false;
        float degrees = 360.0f / (float)NumbScrews;
        numScrewsDone = 0;
        NewEye.GetComponent<EyeMiniGameGoodEyeController>().DesiredLocation = CrackedEye.transform.position;
        NewEye.GetComponent<EyeMiniGameGoodEyeController>().onMag.AddListener(OnFinish);
        screws = new List<EyeMiniGameScrewController>();
        for ( int i = 0; i < NumbScrews; i++)
        {
            GameObject newScrew = GameObject.Instantiate(ScrewPrefab, transform);
            newScrew.transform.position +=
            Quaternion.Euler(0, 0, degrees * (float)i) * new Vector3(Radius, 0, 0) + transform.GetChild(0).localPosition;
            newScrew.GetComponent<EyeMiniGameScrewController>().afterEvent.AddListener(() => ScrewUndone(newScrew));
            screws.Add(newScrew.GetComponent<EyeMiniGameScrewController>());


        }
    }

    void ScrewUndone(GameObject obj)
    {
        this.numScrewsDone = numScrewsDone + 1;
        if (isScrewingIn)
        {
            if (numScrewsDone == NumbScrews)
            {
                GameController.Instance.player.ModifyEyes(50);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Repair Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
        else
        {
            obj.GetComponent<EyeMiniGameScrewController>().hideScrew();
            if (numScrewsDone == NumbScrews)
            {
                CrackedEye.GetComponent<ClickAndDrag>().SetDraggable(true);
                NewEye.GetComponent<EyeMiniGameGoodEyeController>().canBeInstalled = true;
            }
        }
        

    }

    void OnFinish()
    {
        isScrewingIn = true;
        screws.ForEach(item => item.ResetAnimation());
        numScrewsDone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
