using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ClickAndDrag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool canMove = false;
    private Rect location => new Rect(new Vector2(transform.position.x,transform.position.y) - this.GetComponent<RectTransform>().rect.size * .5f * transform.lossyScale, this.GetComponent<RectTransform>().rect.size * transform.lossyScale);
    private bool isDragging;
    private Vector3 lastMouseLocation;
    Vector2 velocity;

    public void SetDraggable(bool value)
    {
        this.canMove = value;
    }
    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;
        if(Input.GetMouseButtonDown(0))
        {
            if(location.Contains(Input.mousePosition))
            {
                isDragging = true;
                lastMouseLocation = Input.mousePosition;
            }
            velocity = Vector2.zero;
            
        }
        if(Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            velocity = (Input.mousePosition - lastMouseLocation) * (1.0f / Time.deltaTime);
        }
        if(isDragging)
        {
            
            this.transform.position += Input.mousePosition - lastMouseLocation;
            lastMouseLocation = Input.mousePosition;
        }
        if (velocity.sqrMagnitude > 2)
            velocity = Vector3.zero;
        velocity = velocity * .9f;
        if (velocity.sqrMagnitude < .1)
            velocity = Vector3.zero;
        this.transform.position += Time.deltaTime * new Vector3(velocity.x, velocity.y, 0) ;    
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(location.position.ToVec3(), location.position.ToVec3() + new Vector3(location.width, 0, 0));
        Gizmos.DrawLine(location.position.ToVec3(), location.position.ToVec3() + new Vector3(0, location.height, 0));
        Gizmos.DrawLine(location.position.ToVec3() + location.size.ToVec3(), location.position.ToVec3() + new Vector3(location.width, 0, 0));
        Gizmos.DrawLine(location.position.ToVec3() + location.size.ToVec3(), location.position.ToVec3() + new Vector3(0, location.height, 0));
    }

}
