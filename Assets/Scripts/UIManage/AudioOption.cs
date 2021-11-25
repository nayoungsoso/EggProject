using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOption : MonoBehaviour
{
    public GameManager gameManager;
    public Slider BGM_Slider;
    public Slider SE_Slider;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        BGM_Slider.value = gameManager.BGM_Volume;
        SE_Slider.value = gameManager.SE_Volume;
    }

    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GamaManager").GetComponent<GameManager>();
        }

        gameManager.BGM_Volume = BGM_Slider.value;
        gameManager.SE_Volume = SE_Slider.value;
    }
}
