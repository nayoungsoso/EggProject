using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject Player;
    public float PositionX;
    public float PositionY;
    public float PositionZ;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    private void Update()
    {
        transform.position = Player.transform.position + new Vector3(PositionX, PositionY, PositionZ);
    }
}