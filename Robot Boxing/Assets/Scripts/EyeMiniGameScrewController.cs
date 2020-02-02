using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeMiniGameScrewController : MonoBehaviour
{
    public bool isUnscrewed;
    public float AnimationTime;
    public float Revolutions;
    public AudioClip soundclip;
    public float audioStartTime;
    public float audioEndTime;
    public UnityEvent afterEvent = new UnityEvent();

    private Rect location => new Rect(new Vector2(transform.position.x, transform.position.y) - this.GetComponent<RectTransform>().rect.size * .5f, this.GetComponent<RectTransform>().rect.size);


    public float currentAnimationTime = 0;
    private bool isAnimating = false;
    private bool hasAnimated = false;
    // Start is called before the first frame update
    void Start()
    {
        currentAnimationTime = AnimationTime;
        hasAnimated = false;
    }

    public void ResetAnimation()
    {
        currentAnimationTime = AnimationTime;
        hasAnimated = false;
    }

    public void hideScrew()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void showScrew()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && location.Contains(Input.mousePosition) && hasAnimated== false)
        {
            currentAnimationTime = 0;
            isAnimating = true;
            hasAnimated = true;
            showScrew();
            if (GetComponent<AudioSource>() != null)
            {
                GetComponent<AudioSource>().clip = soundclip.MakeSubclip(audioStartTime, audioEndTime);
                GetComponent<AudioSource>().Play();
            }
        }
        if(currentAnimationTime < AnimationTime)
        {
        
            float currentRot = ((currentAnimationTime / AnimationTime) * Revolutions) * 360;
            transform.GetChild(1).rotation = Quaternion.Euler(0, 0, currentRot);
            currentAnimationTime += Time.deltaTime;
            
        }
        else if (isAnimating)
        {
            isAnimating = false;

            float currentRot = Revolutions * 360;
            transform.GetChild(1).rotation = Quaternion.Euler(0, 0, currentRot);
            currentAnimationTime += Time.deltaTime;
            afterEvent?.Invoke();
        }
    }
}
