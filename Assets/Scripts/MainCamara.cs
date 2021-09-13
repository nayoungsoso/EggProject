using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamara : MonoBehaviour
{
    public GameObject player;
    Transform playerTrans;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTrans.position, 2f * Time.deltaTime);
        transform.Translate(0, 0, -10); // 카메라를 원래 z축으로 이동
    }
}
