using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeMiniGameGoodEyeController : MonoBehaviour
{
    public float MagDistance = 30;
    public Vector3 DesiredLocation;
    public UnityEvent onMag = new UnityEvent();
    public bool canBeInstalled = false;
    public bool hasReportedInstall = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((DesiredLocation - this.transform.position).sqrMagnitude < MagDistance * MagDistance && canBeInstalled && !hasReportedInstall)
        {
            this.transform.position = DesiredLocation;
            hasReportedInstall = true;
            onMag?.Invoke();
        }
    }
}
