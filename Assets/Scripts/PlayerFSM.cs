using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum State
    {   // 캐릭터 상태
        Earth,  // 지상
        Sky,    // 공중
        Para,   // 낙하산
        Die     // 사망
    }
    // 머신 상태를 저장할 변수 (state)
    public State state;


    float MoveSpeed = 10.0f; // 캐릭터의 이동 속도
    float MaxSpeed = 100.0f; // 캐릭터의 최대 이동 속도
    float JumpPower = 20.0f; // 캐릭터의 점프력
    float JumpCharge = 0.0f; // 캐릭터의 점프를 위해 힘을 충전한 시간
    float ParaPower = 16.0f; // 낙하산에 적용할 바람의 힘
    public static bool IsGrounded = true; // 캐릭터의 지면과의 접촉 상태
    public static bool IsParachute = false; // 캐릭터의 낙하산 상태
    Rigidbody2D rigid;
    Animator anim;

    private void Start()
    {
        state = State.Earth; // 머신의 시작 상태 (Earth)

        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {   // 유한 상태 머신 (FSM)
        switch (state)
        {
            case State.Earth:   // 지상
                Earth(); break;
            case State.Sky:     // 공중
                Sky(); break;
            case State.Para:    // 낙하산
                Para(); break;
            case State.Die:     // 사망
                Die(); break;
        }

        // 버그 방지를 위한 플레이어 속도 제한 (이동, 점프, 낙하 등)
        // Debug.Log(rigid.velocity);
        if (rigid.velocity.x > MaxSpeed)
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < -MaxSpeed)
            rigid.velocity = new Vector2(-MaxSpeed, rigid.velocity.y);
        if (rigid.velocity.y > MaxSpeed)
            rigid.velocity = new Vector2(rigid.velocity.x, MaxSpeed);
        else if (rigid.velocity.y < -MaxSpeed)
            rigid.velocity = new Vector2(rigid.velocity.x, -MaxSpeed);

        // 캐릭터의 지면과의 접촉 상태를 체크
        // Debug.DrawRay(transform.position, Vector3.down * 1.0f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f,LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            IsGrounded = true;
            // Debug.Log("IsGrounded = true");
        }
        else
        {
            IsGrounded = false;
            // Debug.Log("IsGrounded = false");
        }
    }


    // { 상태 정의
    // 지상 상태 정의
    private void Earth()
    {
        Vector2 PlayerPos = transform.position; // 플레이어 포지션
        Vector2 MousePos = Input.mousePosition; // Screen을 기준으로 계산한 마우스 포지션
        Vector2 TransPos = Camera.main.ScreenToWorldPoint(MousePos); // World 좌표를 기준으로 계산한 마우스 포지션
        Vector2 Target = (TransPos - PlayerPos); // 플레이어를 기준으로 계산한 마우스 포지션

        // Earth (상태 정의)
        // Move 정의
        if (Input.GetKey(KeyCode.Z))
        {
            if (Target.x > 0)
                rigid.AddForce(new Vector2(MoveSpeed, 0.0f));
            else if (Target.x < 0)
                rigid.AddForce(new Vector2(-MoveSpeed, 0.0f));
            else { }
        }
        // Jump 정의
        if (Input.GetKey(KeyCode.Space))
        {
            if (JumpCharge < MaxSpeed)
                JumpCharge += Time.deltaTime * JumpPower;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // Debug.Log(JumpPower);
            rigid.AddForce(Target.normalized * JumpCharge, ForceMode2D.Impulse);
            JumpCharge = 0.0f;
        }

        // Earth Escape Trigger (탈출 조건 정의)
        if (!IsGrounded)
        {
            JumpCharge = 0.0f;
            state = State.Sky;
        }
        if (PlayerInteract.CurrentHealth <= 0)
            state = State.Die;
    }

    // 공중 상태 정의
    private void Sky()
    {
        // Sky (상태 정의)
        // Sky Escape Trigger (탈출 조건 정의)
        if (IsGrounded)
            state = State.Earth;
        if (Input.GetKeyDown(KeyCode.X))
            state = State.Para;
        if (PlayerInteract.CurrentHealth <= 0)
            state = State.Die;
    }

    // 낙하산 상태 정의
    private void Para()
    {
        // Para (상태 정의)
        IsParachute = true;
        rigid.AddForce(new Vector2(0.0f, ParaPower));

        // Para Escape Trigger (탈출 조건 정의)
        if (IsGrounded)
        {
            IsParachute = false;
            state = State.Earth;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            IsParachute = false;
            state = State.Sky;
        }
        if (PlayerInteract.CurrentHealth <= 0)
            state = State.Die;
    }

    // 사망 상태 정의
    private void Die()
    {
        // Die (상태 정의)
        // Die Escape Trigger (탈출 조건 정의)
    }
    // } 상태 정의
}