using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_Manager : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource Audio;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Audio = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GamaManager").GetComponent<GameManager>();
        }

        Audio.volume = gameManager.BGM_Volume;
    }
}