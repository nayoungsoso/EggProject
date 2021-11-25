using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeManager : MonoBehaviour
{
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GamaManager").GetComponent<GameManager>();
        }
    }

    public void CostumeChange(Animator anim)
    {
        gameManager.animator.runtimeAnimatorController = anim.runtimeAnimatorController;
    }
}