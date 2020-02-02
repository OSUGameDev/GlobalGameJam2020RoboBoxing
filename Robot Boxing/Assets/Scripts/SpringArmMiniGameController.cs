using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpringArmMiniGameController : MonoBehaviour
{

    public float K = .1f;
    public float clickAmount = .1f;
    public float minimum = .15f;
    public bool isDone;

    private float scale;
    float height;
    float orignalY;
    Vector2 scaledDimenions
        => new Vector2(GetComponent<RectTransform>().rect.width, GetComponent<RectTransform>().rect.height);

    Rect mouseClickBounds
    {
        get {
            Vector2 dim = new Vector2(scaledDimenions.x * transform.lossyScale.x, scaledDimenions.y * transform.lossyScale.y);

            return new Rect(this.transform.position - new Vector3(dim.x, dim.y,0) * .5f, dim);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        isDone = false;
        height = GetComponent<RectTransform>().rect.height;
        orignalY = transform.localPosition.y;
        scale = 1;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0) && mouseOnSpring())
        {
            scale = scale - clickAmount;
        }
        scale = scale + K * (1 - scale) * (1 - scale)* Mathf.Sign(1 - scale);



        if (scale < minimum)
        {
            scale = 0;
            isDone = true;
        }
        scale = scale < 0 ? 0 : scale;
        renderSpring();
    }

    bool mouseOnSpring()
    {
        if(mouseClickBounds.Contains(new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
            return true;
        return false;
        //Vector3 position = Input.mousePosition - (this.transform.position);
        //position = new Vector3(position.x / transform.lossyScale.x, position.y / transform.lossyScale.y, position.z / transform.lossyScale.z);
        //if (position.x < 0 || position.y < 0 || position.x > GetComponent<RectTransform>().rect.width || position.y > GetComponent<RectTransform>().rect.height)
        //    return false;
        //return true;
    }

    void renderSpring()
    {
        this.transform.localScale = new Vector3(transform.localScale.x, scale, transform.localScale.z);
        float newY = orignalY - (height * (1 - scale)) * .5f;
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, newY, this.transform.localPosition.z);
    }
    private void OnDrawGizmos()
    {
        Rect a = mouseClickBounds;
        Vector3 basePose = new Vector3(a.x, a.y, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(basePose, basePose + new Vector3(0,a.height,0));
        Gizmos.DrawLine(basePose, basePose + new Vector3(a.width, 0, 0));
        Gizmos.DrawLine(basePose + new Vector3(a.width, a.height, 0), basePose + new Vector3(a.width, 0, 0));
        Gizmos.DrawLine(basePose + new Vector3(a.width, a.height, 0), basePose + new Vector3(0, a.height, 0));
    }
}
