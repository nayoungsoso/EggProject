using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public static float MaxHealth = 100.0f; // 캐릭터의 최대 체력
    public static float CurrentHealth = 100.0f; // 캐릭터의 현재 체력
    public bool IsDamaged = false; // 공격받는 중인지 확인할 변수
    public static bool IsBurned = false; // 불 오브젝트에 접촉해 있는지 확인할 변수
    public static bool Goal = false; // 목표 도달 여부를 확인할 변수
    float damageDelay = 2.5f; // 공격받은 후에 무적을 유지할 시간
    float Web_Elasticity = 20.0f; // 거미줄의 탄성
    float Spike_Damage = 5.0f; // 가시가 줄 데미지
    float Spike_Flinch = 8.0f; // 가시에서 튕겨져나오는 힘
    float Tornado_Power = 100.0f; // 토네이도의 위로 띄우는 힘
    float Fire_SuddenDeath = 0.0f; // 불 오브젝트에 접촉해 있는 시간
    float Water_Buoyancy = 30.0f; // 물의 부력
    Rigidbody2D rigid; // 캐릭터의 Rigidbody2D
    Animator anim; // 캐릭터의 Animator
    SpriteRenderer sprite; // 캐릭터의 SpriteRenderer

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 애니메이터의 매개변수 'Health'에 캐릭터의 현재 체력 전달
        anim.SetFloat("Health", CurrentHealth);

        // Debug.Log(CurrentHealth); // 현재 캐릭터의 체력 디버그 출력
        // Debug.Log(rigid.velocity.magnitude); // 현재 캐릭터의 속도 디버그 출력
        OnFire();
    }


    // { 오브젝트 상호작용 처리
    private void OnTriggerEnter2D(Collider2D collision)
    { // 오브젝트와 접촉을 시작할 때 (1회성 오브젝트 활성화)
        if (!PlayerFSM.IsDead && !Goal)
        { // 캐릭터 사망 or 목표 도착시 접촉 검사 X
            // 접촉한 오브젝트가 거미줄일 때
            if (collision.tag == "Web")
                OnWeb(collision);
            // 접촉한 오브젝트가 불 또는 용암일 때
            if ((collision.tag == "Fire") || (collision.tag == "Lava"))
            {
                IsBurned = true; // 불 오브젝트에 접촉중임을 체크
                sprite.color = new Color(1.0f, 0.8f, 0.8f, 1.0f); // 캐릭터의 색을 붉은 색으로 변경
            }
        }
        if (!PlayerFSM.IsDead)
        { // 캐릭터 사망시 접촉 검사 X
            if (collision.tag == "Goal")
                FinishLine();
        }
        if (!Goal)
        { // 게임 클리어시 접촉 검사 X
            if (collision.tag == "DeathZone")
                CurrentHealth = 0.0f; // 즉사
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    { // 오브젝트와 접촉중일 때 (지속형 오브젝트 활성화)
        if (!PlayerFSM.IsDead && !Goal)
        { // 캐릭터 사망 or 목표 도착시 접촉 검사 X
            // 접촉중인 오브젝트가 가시일 때
            if (collision.tag == "Spike")
                OnSpike(collision);
            // 접촉중인 오브젝트가 토네이도일 때
            if (collision.tag == "Tornado")
                OnTornado();
        }
        // 캐릭터 상태와 상관 없이 물 오브젝트 상호작용은 작동
        // 접촉중인 오브젝트가 물 또는 용암일 때
        if (collision.tag == "Water" || collision.tag == "Lava")
            UnderWater();
    }

    private void OnTriggerExit2D(Collider2D collision)
    { // 오브젝트와 접촉을 끝낼 때 (지속형 오브젝트 비활성화)
        if (!PlayerFSM.IsDead && !Goal)
        { // 캐릭터 사망 or 목표 도착시 접촉 검사 X
            // 접촉을 끝낸 오브젝트가 불 또는 용암일 때
            if (collision.tag == "Fire" || collision.tag == "Lava")
            {
                IsBurned = false; // 불 오브젝트와 접촉이 끝났음을 체크
                sprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f); // 캐릭터의 색을 원래의 색으로 변경
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { // 캐릭터와 지형의 충돌 검사 (지형 상호작용)

        float crashPower = collision.relativeVelocity.magnitude;

        if (!PlayerFSM.IsDead && !Goal)
        { // 캐릭터 사망 or 목표 도착시 접촉 검사 X
            if (crashPower > 50.0f) // 캐릭터와 지형이 50.0 이상의 속도로 충돌했을 경우
            {
                // Debug.Log(crashPower);
                // 충돌속도(50 ~ 100)의 10분의 1 데미지(5 ~ 10)
                StartCoroutine(Damaged(crashPower / 10.0f, 0.0f, collision.collider));
            }
        }
    }
    // } 오브젝트 상호작용 처리


    // { 오브젝트 상호작용 정의
    private void OnWeb(Collider2D col) // 거미줄 오브젝트 상호작용 정의
    {
        Vector2 BounceTo = transform.position - col.transform.position;
        rigid.AddForce(BounceTo.normalized * Web_Elasticity, ForceMode2D.Impulse);
    }

    private void OnSpike(Collider2D col) // 가시 오브젝트 상호작용 정의
    {
        if (!IsDamaged)
            StartCoroutine(Damaged(Spike_Damage, Spike_Flinch, col));
    }

    private void OnTornado() // 토네이도 오브젝트 상호작용 정의
    {
        float vibrate = Random.Range(-5.0f, 5.0f);
        rigid.AddForce(new Vector2(vibrate, Tornado_Power));
    }

    private void UnderWater() // 물 오브젝트 상호작용 정의
    {
        rigid.AddForce(new Vector2(0.0f, Water_Buoyancy));
    }

    private void OnFire() // 불 오브젝트 상호작용 정의
    {
        if (IsBurned) // 캐릭터가 불 오브젝트와 접촉한 경우
        {
            Fire_SuddenDeath += Time.deltaTime; // 접촉한 시간 계산
            // Debug.Log(Fire_SuddenDeath);

            if (Fire_SuddenDeath >= 5.0f) // 접촉한 시간이 5초가 넘을 경우
                CurrentHealth = 0.0f; // 즉사
        }
        if (!IsBurned) // 캐릭터가 불 오브젝트와 접촉을 끝낸 경우
        {
            Fire_SuddenDeath = 0; // 접촉한 시간 초기화
        }
    }

    IEnumerator Damaged(float damage, float bounce, Collider2D col) // 데미지 코루틴
    {
        IsDamaged = true;
        CurrentHealth -= damage;
        Vector2 BounceTo = transform.position - col.transform.position;
        rigid.AddForce(new Vector2(BounceTo.x, 1.0f) * bounce, ForceMode2D.Impulse);
        sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(damageDelay);
        sprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        IsDamaged = false;
    }

    private void FinishLine() // 목표 도착 확인 오브젝트 상호작용 정의
    {
        // 목표 도착 여부 true
        Goal = true;
    }
    // } 오브젝트 상호작용 정리
}