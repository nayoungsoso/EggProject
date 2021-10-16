using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Slider hpSlider; // 체력바 슬라이더

    public AudioClip hitClip; // 피격 소리

    public float startHp = 100;
    public float hp;
    public PlayerMove playerMove;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        hp = startHp;
        hpSlider.gameObject.SetActive(true);
        hpSlider.maxValue = startHp;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = hp;
    }

    public void OnDamage(float damage)
    {
        print("damage");
        if (hp > 0)
        {
            hp -= damage;
        }
        if (hp < 1)
        {
            GameObject.Find("Player").GetComponent<PlayerMove>().OnDie();
        }
    }
}
