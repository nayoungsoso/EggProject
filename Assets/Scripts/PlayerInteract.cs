using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public static float MaxHealth = 100;
    public static float CurrentHealth = 100;
    bool isDamaged = false;
    bool isBurned = false;
    float damageDelay = 3.0f;
    float Web_Elasticity = 20.0f;
    float Spike_Damage = 5.0f;
    float Spike_Flinch = 5.0f;
    float Tornado_Power = 80.0f;
    float Fire_SuddenDeath = 0.0f;
    float Water_Buoyancy = 30.0f;
    Rigidbody2D rigid;
    Animator anim;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetFloat("Health", CurrentHealth);

        if (isBurned)
        {
            Fire_SuddenDeath += Time.deltaTime;
            Debug.Log(Fire_SuddenDeath);

            if (Fire_SuddenDeath >= 5.0f)
                CurrentHealth = 0.0f;
        }
        if (!isBurned)
            Fire_SuddenDeath = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { // 오브젝트와 접촉을 시작할 때 (1회성 오브젝트 활성화)
        if (collision.tag == "Web")
            OnWeb(collision);
        if ((collision.tag == "Fire") || (collision.tag == "Lava"))
            isBurned = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    { // 오브젝트와 접촉중일 때 (지속형 오브젝트 활성화)
        if (collision.tag == "Spike")
            OnSpike(collision);
        if (collision.tag == "Tornado")
            OnTornado();
        if (collision.tag == "Water" || collision.tag == "Lava")
            InWater();
    }

    private void OnTriggerExit2D(Collider2D collision)
    { // 오브젝트와 접촉을 끝낼 때 (지속형 오브젝트 비활성화)
        if (collision.tag == "Fire" || collision.tag == "Lava")
            isBurned = false;
    }

    private void OnWeb(Collider2D col)
    {
        Vector2 BounceTo = transform.position - col.transform.position;
        rigid.AddForce(BounceTo.normalized * Web_Elasticity, ForceMode2D.Impulse);
    }

    private void OnSpike(Collider2D col)
    {
        Vector2 BounceTo = transform.position - col.transform.position;
        if (!isDamaged)
            DamageDelay(Spike_Damage);
        rigid.AddForce(new Vector2(BounceTo.x, 1.0f) * Spike_Flinch, ForceMode2D.Impulse);
    }

    private void OnTornado()
    {
        rigid.AddForce(new Vector2(0.0f, Tornado_Power));
    }

    private void InWater()
    {
        rigid.AddForce(new Vector2(0.0f, Water_Buoyancy));
    }

    IEnumerable DamageDelay(float damage)
    {
        isDamaged = true;
        yield return new WaitForSeconds(damageDelay);
        CurrentHealth -= damage;
        isDamaged = false;
    }
}