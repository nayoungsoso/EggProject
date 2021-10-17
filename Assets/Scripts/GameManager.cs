using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Sprite crack1;
    public Sprite crack2;
    public static GameManager instance;
    public Slider hpSlider; // 체력바 슬라이더
    SpriteRenderer spriteRenderer;

    public AudioClip hitClip; // 피격 소리

    public float startHp = 100;
    public float hp;

    GameObject deadPoint;
    Vector3 deadPosition;
    public GameObject deadEgg;
    public GameObject burnEgg;
    public Transform deadRoot;

    private void Awake()
    {
        instance = this;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
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
        if (hp < 50)
        {
            spriteRenderer.sprite = crack1;
        }
        else if (hp < 10)
        {
            spriteRenderer.sprite = crack2;
        }
        if (hp < 1)
        {
            deadPosition = player.transform.localPosition;
            deadPosition.y += -0.1f;
            if (GameObject.Find("Player").GetComponent<PlayerMove>().burn)
            {
                deadPoint = Instantiate(burnEgg, deadRoot);
            }
            else
            {
                deadPoint = Instantiate(deadEgg, deadRoot);
            }
            deadPoint.transform.position = deadPosition;
            
            GameObject.Find("Player").GetComponent<PlayerMove>().OnDie();
        }
    }
}
