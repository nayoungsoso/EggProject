using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyGuide : MonoBehaviour
{
    Image zKey;
    Image xKey;
    Image spaceKey;

    void Start()
    {
        zKey = GameObject.Find("Z").GetComponent<Image>();
        xKey = GameObject.Find("X").GetComponent<Image>();
        spaceKey = GameObject.Find("Space bar").GetComponent<Image>();
    }

    void Update()
    {
        // KeyDown 이벤트 처리
        if (Input.GetKeyDown(KeyCode.Z))
            zKey.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        if (Input.GetKeyDown(KeyCode.X))
            xKey.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        if (Input.GetKeyDown(KeyCode.Space))
            spaceKey.color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

        // KeyUp 이벤트 처리
        if (Input.GetKeyUp(KeyCode.Z))
            zKey.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (Input.GetKeyUp(KeyCode.X))
            xKey.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if (Input.GetKeyUp(KeyCode.Space))
            spaceKey.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}