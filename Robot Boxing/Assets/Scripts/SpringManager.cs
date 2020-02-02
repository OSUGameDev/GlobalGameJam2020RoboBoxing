using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpringManager : MonoBehaviour
{
    public List<GameObject> springs;
    public float AnimationTime;
    private float currentAnimationTime;
    private bool isAnimatiing;

    public Sprite OpenImage;
    public Sprite ClosedImage;
    public GameObject backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        currentAnimationTime = 0;
        isAnimatiing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(springs.All(item => item.GetComponent<SpringArmMiniGameController>().isDone))
            {
                isAnimatiing = true;
                backgroundImage.GetComponent<Image>().sprite = ClosedImage;
            }
        }

        if(isAnimatiing)
        {
            currentAnimationTime += Time.deltaTime;
            if(currentAnimationTime >= AnimationTime)
            {
                GameController.Instance.player.ModifyArm(50f);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Repair Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}
