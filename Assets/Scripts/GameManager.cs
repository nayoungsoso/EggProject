using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float BGM_Volume = 1.0f;
    public float SE_Volume = 1.0f;

    // 현재 선택한 캐릭터의 외형을 저장할 애니메이터
    public Animator animator;

    // 게임오브젝트 싱글톤 선언
    private static GameManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        animator = GetComponent<Animator>();
    }
}