using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParaManager : MonoBehaviour
{
    public GameObject Player;
    public float PositionX;
    public float PositionY;
    public float PositionZ;

    private void Start()
    {
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        transform.position = Player.transform.position + new Vector3(PositionX, PositionY, PositionZ);

        if (PlayerFSM.IsParachute)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        else
            transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
}