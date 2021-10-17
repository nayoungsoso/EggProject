using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    float x, y;

    void Start()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        Vector2 pos = GameObject.FindWithTag("Player").transform.position;
        x = pos.x;
        y = pos.y + 0.5f;
        transform.position = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
        
    }
}
