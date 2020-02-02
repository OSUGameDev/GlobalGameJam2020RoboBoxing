using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMiniGameController : MonoBehaviour
{

    public float Radius;
    public int NumbScrews;
    public GameObject ScrewPrefab;
    private int numUnscrewed;
    // Start is called before the first frame update
    void Start()
    {
        float degrees = 360.0f / (float)NumbScrews;
        numUnscrewed = 0;
        for ( int i = 0; i < NumbScrews; i++)
        {
            GameObject newScrew = GameObject.Instantiate(ScrewPrefab, transform);
            newScrew.transform.position +=
            Quaternion.Euler(0, 0, degrees * (float)i) * new Vector3(Radius, 0, 0);
            newScrew.GetComponent<EyeMiniGameScrewController>().afterEvent.AddListener(ScrewUndone);
            
        }
    }

    void ScrewUndone()
    {
        numUnscrewed++;
        if(numUnscrewed == NumbScrews)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
