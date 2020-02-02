using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage_behavior : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float launchForce = 1;
    public string text = "0";
    private GameObject child;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(launchForce,launchForce));
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        child.GetComponent<Text>().text = text;
    }
}
