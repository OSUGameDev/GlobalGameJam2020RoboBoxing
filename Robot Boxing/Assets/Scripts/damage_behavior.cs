using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage_behavior : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float launchForce = 500;
    public string text = "0";
    public float aliveTime = 1;
    private float timer = 0;
    private GameObject child;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float xRand = Random.Range(-5,5);
        float yRand = Random.Range(-5,5);
        rb.AddForce(new Vector2(launchForce*xRand,launchForce*yRand));
        child = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        child.GetComponent<Text>().text = text;
        timer+=Time.deltaTime;
        if(timer >= aliveTime)
            GameObject.Destroy(this);
    }
}
