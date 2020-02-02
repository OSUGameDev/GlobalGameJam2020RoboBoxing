using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtendor
{
    public static Vector3 ComponetMultiply(this Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

    public static AudioClip MakeSubclip(this AudioClip clip, float start, float stop)
    {
        /* Create a new audio clip */
        int frequency = clip.frequency;
        float timeLength = stop - start;
        int samplesLength = (int)(frequency * timeLength);
        AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, 1, frequency, false);
        /* Create a temporary buffer for the samples */
        float[] data = new float[samplesLength];
        /* Get the data from the original clip */
        clip.GetData(data, (int)(frequency * start));
        /* Transfer the data to the new clip */
        newClip.SetData(data, 0);
        /* Return the sub clip */
        return newClip;
    }
}

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
            Quaternion.Euler(0, 0, degrees * (float)i) * new Vector3(Radius, 0, 0).ComponetMultiply(transform.GetChild(0).transform.lossyScale) 
            + transform.GetChild(0).localPosition.ComponetMultiply(transform.GetChild(0).transform.lossyScale);
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
