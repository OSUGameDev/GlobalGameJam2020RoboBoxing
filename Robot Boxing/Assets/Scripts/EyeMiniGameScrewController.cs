using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeMiniGameScrewController : MonoBehaviour
{
    public bool isUnscrewed;
    public float AnimationTime;
    public float Revolutions;
    public UnityEvent afterEvent;

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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && location.Contains(Input.mousePosition) && hasAnimated== false)
        {
            currentAnimationTime = 0;
            isAnimating = true;
            hasAnimated = true;
        }
        if(currentAnimationTime < AnimationTime)
        {
            float currentRot = ((currentAnimationTime / AnimationTime) * Revolutions) * 360;
            this.transform.rotation = Quaternion.Euler(0, 0, currentRot);
            currentAnimationTime += Time.deltaTime;
        }
        else if (isAnimating)
        {
            isAnimating = false;

            float currentRot = Revolutions * 360;
            this.transform.rotation = Quaternion.Euler(0, 0, currentRot);
            currentAnimationTime += Time.deltaTime;
            afterEvent?.Invoke();
        }
    }
}
