using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parachute : MonoBehaviour
{
    float x, y;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = GameObject.FindWithTag("Player").transform.position;
        x = pos.x;
        y = pos.y + 0.5f;
        transform.position = new Vector2(x, y);
    }
}
